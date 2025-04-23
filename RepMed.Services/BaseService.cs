using RepMed.Core;
using RepMed.Dtos;
using RepMed.Localize;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace RepMed.Services
{
    public class BaseService
    {
        protected internal IDbConnection _idbConnection;
        protected internal IDbTransaction _idbTransaction;
        protected readonly EmailSettings _emailSettings;
        protected internal AppSettings _appSettings;
        protected internal AdminSettings _adminSettings;
        protected internal IGenericMapper _mapper;
        protected internal IValidationMessagesServices _validateMessages;
        protected internal IHttpContextAccessor _httpContext;
        public BaseService(IDbConnection sqlConnection, IDbTransaction dbTransaction,
            IOptions<AppSettings> appSettings, IHttpContextAccessor httpContext,
            IOptions<EmailSettings> emailSettings = null
            )
        {
            _httpContext = httpContext;
            _idbConnection = sqlConnection;
            _idbTransaction = dbTransaction;
            _emailSettings = emailSettings?.Value;
            _appSettings = appSettings.Value;
            _mapper = ServiceActivator.GetScope().ServiceProvider.GetService<IGenericMapper>();
        }




        public BaseService(IDbConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _idbConnection = sqlConnection;
            _idbTransaction = dbTransaction;
            _mapper = ServiceActivator.GetScope().ServiceProvider.GetService<IGenericMapper>();
            _appSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AppSettings>>().Value;
            _adminSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<AdminSettings>>().Value;
            _emailSettings = ServiceActivator.GetScope().ServiceProvider.GetService<IOptions<EmailSettings>>().Value;
            _validateMessages = ServiceActivator.GetScope().ServiceProvider.GetService<IValidationMessagesServices>();
        }

        public Task<bool> IsEmailExist(string email, bool checkUserTbl = false)
        {
            string sqlExist = DbTables.tblPersons.Select(
                                new string[] { nameof(BasicPersonsDto.Email) },
                                whereQuery: $@" `{nameof(BasicPersonsDto.Email)}` = '{email}' ");

            if (checkUserTbl)
            {
                string whereUser = DbTables.tblUser.Select(new string[] { nameof(BasicPersonsDto.Email) }, tblPrefix: "u",
                    whereQuery: $@" u.`{nameof(BasicPersonsDto.Email)}` = '{email}' ");

                sqlExist = DbTables.tblPersons.Select(new string[] { nameof(BasicPersonsDto.Email) },
                    whereQuery: $@" `{nameof(BasicPersonsDto.Email)}` = ({whereUser}) ");
            }

            BasicPersonsDto isExist = _idbConnection.Query<BasicPersonsDto>(sqlExist, transaction: _idbTransaction).SingleOrDefault();
            if (isExist != null)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
        public Task<bool> IsPhoneExist(string phoneNo, bool checkUserTbl = false)
        {
            string sqlExist = DbTables.tblPersons.Select(new string[] { nameof(BasePersonDto.Mobile) }, whereQuery: $@" ""{nameof(BasePersonDto.Mobile)}"" = '{phoneNo}' ");

            BasicPersonsDto isExist = _idbConnection.Query<BasicPersonsDto>(sqlExist, transaction: _idbTransaction).SingleOrDefault();
            if (isExist != null)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
        public Task<bool> IsPharmacyFieldExist(string fieldName, string fieldValue)
        {
            // Sanitize column name to prevent SQL injection from fieldName
            var validFields = new[] { "LicenseNumber", "GSTNumber", "OfficialEmail", "RegisteredMobile" };
            if (!validFields.Contains(fieldName))
                throw new ArgumentException("Invalid field name");
            string sql = $@"
                SELECT `{fieldName}`
                FROM {DbTables.tblPharmacy}
                WHERE `{fieldName}` = @Value
                LIMIT 1;";

            var result = _idbConnection.QueryFirstOrDefault<string>(sql, new { Value = fieldValue }, transaction: _idbTransaction);

            return Task.FromResult(result != null);
        }
        public Task<bool> IsAccountNumberExist(string accountNumber)
        {
            string sql = $@"
                SELECT 1
                FROM {DbTables.tblPharmacyBankDetails}
                WHERE AccountNumber = @AccountNumber
                LIMIT 1;";

            var result = _idbConnection.ExecuteScalar(sql, new { AccountNumber = accountNumber }, transaction: _idbTransaction);

            return Task.FromResult(result != null);
        }
        public Task<bool> IsRoleExist(string roleName)
        {
            string sql = $@"
                SELECT 1
                FROM {DbTables.tblRole}
                WHERE RoleName = @RoleName
                LIMIT 1;";

            var result = _idbConnection.ExecuteScalar(sql, new { RoleName = roleName }, transaction: _idbTransaction);

            return Task.FromResult(result != null);
        }


        public bool IsUserExist(Guid? userid)
        {
            string sqlExist = DbTables.tblUser.Select(
                                new string[] { nameof(BasicPersonsDto.Email) },
                                whereQuery: $@" ""Id"" = '{userid}' ");

            BasicPersonsDto isExist = _idbConnection.Query<BasicPersonsDto>(sqlExist, transaction: _idbTransaction).SingleOrDefault();
            if (isExist != null)
                return true;

            return false;
        }
    }
}
