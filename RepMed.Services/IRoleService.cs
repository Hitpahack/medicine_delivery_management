using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RepMed.Core.Enums;

namespace RepMed.Services
{
    public interface IRoleService : IDisposable
    {
        Task<APIsResponse<EntityRoleDto>> CreateRole(CreateRoleDto reqDto);
        Task<APIsResponse<bool>> UpdateRolePermission(CreateRoleDto reqDto, long Id);
        Task<APIsResponse<bool>> GetRole(long roleId);
        Task<APIsResponse<List<EntityPermissionDto>>> GetAllPermissions();
    }
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityRoleDto>> CreateRole(CreateRoleDto reqDto)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                #region Check RoleExist
                if ((await IsRoleExist(reqDto.RoleName)))
                {
                    return await Task.FromResult(new APIsError<EntityRoleDto>(_validateMessages.AlreadyExist));
                }
                #endregion
                #region Add Role
                EntityRoleDto response = _idbConnection.Insert<EntityRoleDto>(_idbTransaction,
                    DbTables.tblRole,
                    DapperHelper.QueryAsColumnsParma<Role, CreateRoleDto>(),
                    DapperHelper.QueryAsValuesParma<Role, CreateRoleDto>(),
                    reqDto);
                #endregion
                #region 
                if (reqDto.PermissionIds != null && reqDto.PermissionIds.Any())
                {
                    foreach (var permissionId in reqDto.PermissionIds)
                    {
                        await _idbConnection.ExecuteAsync(
                            $@"INSERT INTO {DbTables.tblRolePermissions} (RoleId, PermissionId) VALUES (@RoleId, @PermissionId);",
                            new { RoleId = response.Id, PermissionId = permissionId }, _idbTransaction);
                    }
                }
                #endregion
                apiResponse = new APIsSuccsss<EntityRoleDto>(_validateMessages.AddSuccess, response);
                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityRoleDto>(ex.GetActualError()));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<APIsResponse<List<EntityPermissionDto>>> GetAllPermissions()
        {
            try
            {
                string query = DbTables.tblPermissions.SelectAll();
                var rolepermissions = await _idbConnection.QueryAsync<EntityPermissionDto>(query, transaction: _idbTransaction);
                return new APIsSuccsss<List<EntityPermissionDto>> (_validateMessages.RetriveSuccess,rolepermissions);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<EntityPermissionDto>> (ex.GetActualError()));
            }


        }

        public Task<APIsResponse<bool>> GetRole(long roleId)
        {
            throw new NotImplementedException();    
            //try
            //{
            //    var sql = $@"
            //                SELECT 
            //                FROM {DbTables.tblRole} r
            //                INNER JOIN {DbTables.tblPersons} p ON u.PersonId = p.Id
            //                INNER JOIN {DbTables.tblUserAddress} a ON a.PersonId = p.Id
            //                WHERE u.PersonId = @Id;
            //            ";
            //    var result = await _idbConnection.QueryAsync<GetUserDto, BasicAddressDto, GetUserDto>(
            //                    sql,
            //                    (user, address) =>
            //                    {
            //                        user.Address = address;
            //                        return user;
            //                    },
            //                    new { Id = personid },
            //                    splitOn: "AddressLine", // this tells Dapper where to start splitting the object
            //                    transaction: _idbTransaction
            //                );

            //    var userData = result.FirstOrDefault();
            //    if (userData == null)
            //        return new APIsError<GetUserDto>(_validateMessages.NotExist);

            //    return new APIsSuccsss<GetUserDto>(_validateMessages.RetriveSuccess, userData);

            //}
            //catch (Exception ex)
            //{
            //    return await Task.FromResult(new APIsError<GetUserDto>(ex.GetActualError()));
            //}
        }

        public async Task<APIsResponse<bool>> UpdateRolePermission(CreateRoleDto reqDto, long Id)
        {
            try
            {
                APIsResponse<bool> apiResponse = default(APIsResponse<bool>);
                #region Remove All Permission of current role
                await _idbConnection.ExecuteAsync($@"DELETE FROM {DbTables.tblRolePermissions} WHERE RoleId = @RoleId",new { RoleId = Id }, _idbTransaction);
                #endregion
                #region Add New Permission
                if (reqDto.PermissionIds != null && reqDto.PermissionIds.Any())
                {
                    foreach (var permissionId in reqDto.PermissionIds)
                    {
                        await _idbConnection.ExecuteAsync(
                            $@"INSERT INTO {DbTables.tblRolePermissions} (RoleId, PermissionId) VALUES (@RoleId, @PermissionId);",
                            new { RoleId = Id, PermissionId = permissionId }, _idbTransaction);
                    }
                }
                #endregion
                apiResponse = new APIsSuccsss<bool>(_validateMessages.UpdateSuccess);
                return await Task.FromResult(apiResponse);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
            }
        }        
    }
}
