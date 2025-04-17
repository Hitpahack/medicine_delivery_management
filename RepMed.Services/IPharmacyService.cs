using Dapper;
using Microsoft.AspNetCore.Mvc;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.PharmacyPage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace RepMed.Services
{
    public interface IPharmacyService : IDisposable
    {
        Task<APIsResponse<PharmacyBankDetailsDto>> AddUpdatePharmacyBankDetails(PharmacyBankDetailsDto reqDto, long Id);
        Task<APIsResponse<AddPharmacyDto>> AddUpdatePharmacy(AddPharmacyDto reqDto, long Id);
        Task<APIsResponse<Datatable<PharmacyPagingResponse>>> GetAllPharmacies(PharmacyPagingRequest reqDto);
        Task<APIsResponse<BasePharmacyDto>> GetPharmacy(long Id);
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
                    #region Update Pharmacy Bank Details
                    var pharmacyBank = _idbConnection.Update<PharmacyBankDetailsDto>(_idbTransaction,DbTables.tblPharmacyBankDetails,
                                       new Dictionary<string, string> {
                                              { "BankName", reqDto.BankName },
                                              { "AccountHolderName", reqDto.AccountHolderName },
                                              { "AccountNumber", reqDto.AccountNumber },
                                              { "Ifsccode", reqDto.Ifsccode },
                                              { "BranchName", reqDto.BranchName },
                                              { "UpiId", reqDto.UpiId },
                                       },
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
                DateTime expiry = DateTime.UtcNow.AddHours(12); // Token valid for 12 hours

                #region create user token for set password
                string sql = $@"INSERT INTO {DbTables.tblUserTokens} (UserId, Token, ExpiresAt) VALUES (@UserId, @Token, @ExpiresAt)";
                await _idbConnection.ExecuteAsync(
                            sql,
                            new
                            {
                                UserId = newUserId,
                                Token = token,
                                ExpiresAt = expiry
                            }, transaction: _idbTransaction);
                #endregion
                #region send email for set password
                string url = $"https://localhost:44379/set-password?token={token}";

                await SendEmailAsync(userEmail, "Set Your Password", $"Click here to set your password: <a href='{url}'>Set Password</a>");
                #endregion

                return new APIsSuccsss<bool>("Email Sent for genrate password", true);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }

        }
        private async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody)
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
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
                    apiResponse = new APIsSuccsss<AddPharmacyDto>(_validateMessages.AddSuccess, pharmacy);
                }
                else
                {
                    #region Check Pharmacy Exist
                    string sql = $@"SELECT a.Id FROM {DbTables.tblPharmacy} a WHERE a.{nameof(Pharmacy.Id)} = {Id}";
                    var pharmacyData = await _idbConnection.QueryFirstOrDefaultAsync<AddPharmacyDto>(sql, transaction: _idbTransaction);
                    if (pharmacyData == null)
                    {
                        apiResponse = new APIsSuccsss<AddPharmacyDto>(_validateMessages.NotExist);
                    }
                    #endregion
                    #region Update Pharmacy
                    var pharmacy = _idbConnection.Update<AddPharmacyDto>(_idbTransaction,DbTables.tblPharmacy,
                                    new Dictionary<string, string> {
                                          { "LicenseNumber", reqDto.LicenseNumber },
                                          { "LicenseExpiry", reqDto.LicenseExpiry?.ToString("yyyy-MM-dd HH:MM:ss") },
                                          { "StoreMobile1", reqDto.StoreMobile1 },
                                          { "StoreEmail1", reqDto.StoreEmail1 },
                                          { "StoreEmail2", reqDto.StoreEmail2 },
                                          { "Address1", reqDto.Address1 },
                                          { "Address2", reqDto.Address2 },
                                          { "Latitude", reqDto.Latitude.ToString() },
                                          { "Longitude", reqDto.Longitude.ToString() },
                                          { "CityId", reqDto.CityId.ToString() },
                                          { "StateId", reqDto.StateId.ToString() },
                                          { "CountryId", reqDto.CountryId.ToString() },
                                    },
                                    reqDto.Id);
                    #endregion
                    apiResponse = new APIsSuccsss<AddPharmacyDto>(_validateMessages.UpdateSuccess, pharmacy);

                }
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<AddPharmacyDto>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<Datatable<PharmacyPagingResponse>>> GetAllPharmacies(PharmacyPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<PharmacyPagingResponse>> apiResponse = default;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", reqDto.order_by, DbType.String);

                var result = (await _idbConnection.QueryAsync<PharmacyPagingResponse>(
                               sql: "GET_PHARMACY_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<PharmacyPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                #region Get All Pharmacy Commented

                //var sql = @"
                //            SELECT *                             
                //            FROM pharmacies p
                //            LEFT JOIN pharmacybankdetails b ON p.Id = b.PharmacyId";
                //var pharmacyBank = await _idbConnection.QueryAsync<AddPharmacyDto, PharmacyBankDetailsDto, BasePharmacyDto>(
                //        sql,
                //        (pharmacy, bankDetails) => new BasePharmacyDto
                //        {
                //            Pharmacy = pharmacy,
                //            PharmacyBankDetails = bankDetails
                //        },
                //        transaction: _idbTransaction,
                //        splitOn: "PharmacyId"
                //);
                #endregion
                if(result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<PharmacyPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<PharmacyPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<PharmacyPagingResponse>>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<BasePharmacyDto>> GetPharmacy(long Id)
        {
            try
            {
                APIsResponse<BasePharmacyDto> apiResponse = default;
                #region Get All Pharmacy Commented
                var sql = $@"
                            SELECT *                             
                            FROM {DbTables.tblPharmacy} p
                            LEFT JOIN {DbTables.tblPharmacyBankDetails} b ON p.Id = b.PharmacyId where p.Id = {Id}";
                var pharmacyDataList = (await _idbConnection.QueryAsync<AddPharmacyDto, PharmacyBankDetailsDto, BasePharmacyDto>(
                                        sql,
                                        (pharmacy, bankDetails) => new BasePharmacyDto
                                        {
                                            Pharmacy = pharmacy,
                                            PharmacyBankDetails = bankDetails
                                        },
                                        transaction: _idbTransaction,
                                        splitOn: "PharmacyId"
                                    )).ToList();

                var pharmacyData = pharmacyDataList.FirstOrDefault();
                #endregion
                if (pharmacyData !=null)
                    apiResponse = new APIsSuccsss<BasePharmacyDto>(_validateMessages.RetriveSuccess, pharmacyData);
                else
                    return new APIsSuccsss<BasePharmacyDto>(_validateMessages.NotExist);
                return await Task.FromResult(apiResponse);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<BasePharmacyDto>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
