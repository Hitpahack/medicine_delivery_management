using Dapper;
using MimeKit;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IPharmacyService : IDisposable
    {
        Task<APIsResponse<PharmacyBankDetailsDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id);
        Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id);
        Task<APIsResponse<IEnumerable<BasePharmacyDto>>> GetAllPharmacies();
        Task<APIsResponse<bool>> GenrateEmailToken(long newUserId, string userEmail);
    }

    public class PharmacyService : BaseService, IPharmacyService
    {

        public PharmacyService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<PharmacyBankDetailsDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id)
        {
            try
            {
                APIsResponse<PharmacyBankDetailsDto> apiResponse = default;

                if (Id == 0)
                {
                    #region Add Pharmacy Bank Details
                    var pharmacyBank = _idbConnection.Insert<PharmacyBankDetailsDto>(_idbTransaction,
                                   DbTables.tblPharmacyBankDetails,
                                   DapperHelper.QueryAsColumnsParma<Pharmacybankdetail, PharmacyBankDetailsDto>(),
                                   DapperHelper.QueryAsValuesParma<Pharmacybankdetail, PharmacyBankDetailsDto>(),
                                   reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<PharmacyBankDetailsDto>("Pharmacy bank details added successfully", pharmacyBank);
                }
                else
                {
                    #region Update Pharmacy
                    var pharmacyBank = _idbConnection.Update<AddPharmacyDto>(
                                    _idbTransaction,
                                    DbTables.tblPharmacy,
                                    DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                    reqDto,
                                    reqDto.Id);
                    #endregion
                    apiResponse = new APIsSuccsss<PharmacyBankDetailsDto>("Pharmacy bank details Updated successfully", pharmacyBank);


                }
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<PharmacyBankDetailsDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<bool>> GenrateEmailToken(long newUserId, string userEmail)
        {
            try
            {
                string token = Guid.NewGuid().ToString();
                DateTime expiry = DateTime.UtcNow.AddHours(12); // Token valid for 2 hours

                string sql = $@"INSERT INTO {DbTables.tblUserTokens} (UserId, Token, ExpiresAt) VALUES (@UserId, @Token, @ExpiresAt)";
                await _idbConnection.ExecuteAsync(
                            sql,
                            new
                            {
                                UserId = newUserId,
                                Token = token,
                                ExpiresAt = expiry
                            }, transaction: _idbTransaction);

                string url = $"https://localhost:44379/set-password?token={token}";

                await SendEmailAsync(userEmail, "Set Your Password", $"Click here to set your password: <a href='{url}'>Set Password</a>");

                return new APIsSuccsss<bool>("Email Sent for genrate password", true);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }

        }
        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                var fromEmail = _emailSettings.FromEmail;
                var fromName = _emailSettings.FromName;
                var username = _emailSettings.UsernameEmail;
                var password = _emailSettings.UsernamePassword;
                var smtpHost = _emailSettings.PrimaryDomain;
                var smtpPort = _emailSettings.PrimaryPort;
                var enableSsl = _emailSettings.EnableSSL;

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.EnableSsl = enableSsl;
                    smtp.Credentials = new NetworkCredential(username, password);

                    var mail = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName),
                        Subject = subject,
                        Body = htmlBody,
                        IsBodyHtml = true
                    };
                    mail.To.Add(toEmail);

                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {

            }
        }



        public async Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id)
        {
            try
            {
                APIsResponse<AddPharmacyDto> apiResponse = default;

                if (Id == 0)
                {
                    #region Add Pharmacy
                    var pharmacy = _idbConnection.Insert<AddPharmacyDto>(_idbTransaction,
                                   DbTables.tblPharmacy,
                                   DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                   DapperHelper.QueryAsValuesParma<Pharmacy, AddPharmacyDto>(),
                                   reqDto);
                    #endregion
                    apiResponse = new APIsSuccsss<AddPharmacyDto>("Pharmacy created successfully", pharmacy);
                }
                else
                {

                    #region Update Pharmacy
                    var pharmacy = _idbConnection.Update<AddPharmacyDto>(
                                    _idbTransaction,
                                    DbTables.tblPharmacy,
                                    DapperHelper.QueryAsColumnsParma<Pharmacy, AddPharmacyDto>(),
                                    reqDto,
                                    reqDto.Id);
                    #endregion
                    apiResponse = new APIsSuccsss<AddPharmacyDto>("Pharmacy Updated successfully", pharmacy);

                }
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<AddPharmacyDto>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<IEnumerable<BasePharmacyDto>>> GetAllPharmacies()
        {
            try
            {
                APIsResponse<IEnumerable<BasePharmacyDto>> apiResponse = default;

                #region Get All Pharmacy 
                var sql = @"
                            SELECT *                             
                            FROM pharmacies p
                            LEFT JOIN pharmacybankdetails b ON p.Id = b.PharmacyId";
                var pharmacyBank = await _idbConnection.QueryAsync<AddPharmacyDto, PharmacyBankDetailsDto, BasePharmacyDto>(
                        sql,
                        (pharmacy, bankDetails) => new BasePharmacyDto
                        {
                            Pharmacy = pharmacy,
                            PharmacyBankDetails = bankDetails
                        },
                        transaction: _idbTransaction,
                        splitOn: "PharmacyId"
                );
                #endregion
                if (pharmacyBank.Any())
                    apiResponse = new APIsSuccsss<IEnumerable<BasePharmacyDto>>("Pharmacy details retrieved successfully", pharmacyBank);
                else
                    apiResponse = new APIsSuccsss<IEnumerable<BasePharmacyDto>>("No Pharmacy Found", pharmacyBank);


                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<IEnumerable<BasePharmacyDto>>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
