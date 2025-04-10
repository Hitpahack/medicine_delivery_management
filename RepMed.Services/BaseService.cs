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
        public BaseService(IDbConnection sqlConnection, IDbTransaction dbTransaction,
            IOptions<AppSettings> appSettings,
            IOptions<EmailSettings> emailSettings = null
            )
        {
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
                                new string[] { nameof(BasePerson.Email) },
                                whereQuery: $@" `{nameof(BasePerson.Email)}` = '{email}' ");

            if (checkUserTbl)
            {
                string whereUser = DbTables.tblUser.Select(new string[] { nameof(BasePerson.Email) }, tblPrefix: "u",
                    whereQuery: $@" u.`{nameof(BasePerson.Email)}` = '{email}' ");

                sqlExist = DbTables.tblPersons.Select(new string[] { nameof(BasePerson.Email) },
                    whereQuery: $@" `{nameof(BasePerson.Email)}` = ({whereUser}) ");
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

        public bool IsUserExist(Guid? userid)
        {
            string sqlExist = DbTables.tblUser.Select(
                                new string[] { nameof(BasePerson.Email) },
                                whereQuery: $@" ""Id"" = '{userid}' ");

            BasicPersonsDto isExist = _idbConnection.Query<BasicPersonsDto>(sqlExist, transaction: _idbTransaction).SingleOrDefault();
            if (isExist != null)
                return true;

            return false;
        }
    }
}
