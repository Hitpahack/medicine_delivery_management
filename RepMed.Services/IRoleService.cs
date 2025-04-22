using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IRoleService : IDisposable
    {
        Task<APIsResponse<EntityRoleDto>> CreateRole(BasicRoleDto reqDto);
    }
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityRoleDto>> CreateRole(BasicRoleDto reqDto)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                #region Check UserExist

                #endregion
                #region Add Role
                EntityRoleDto response = _idbConnection.Insert<EntityRoleDto>(_idbTransaction,
                    DbTables.tblRole,
                    DapperHelper.QueryAsColumnsParma<Role, BasicRoleDto>(),
                    DapperHelper.QueryAsValuesParma<Role, BasicRoleDto>(),
                    reqDto);
                #endregion
                apiResponse = new APIsSuccsss<EntityRoleDto>(_validateMessages.RetriveSuccess, response);



                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityRoleDto>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
