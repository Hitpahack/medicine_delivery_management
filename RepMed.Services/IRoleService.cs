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
        Task<APIsResponse<EntityRoleDto>> AddEditRole(CreateRoleDto reqDto, long Id);
        Task<APIsResponse<List<EntityPermissionDto>>> GetRole(long roleId);
        Task<APIsResponse<List<EntityPermissionDto>>> GetAllPermissions();
    }
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityRoleDto>> AddEditRole(CreateRoleDto reqDto , long Id)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                if (Id == 0)
                {


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
                    else
                        return new APIsError<EntityRoleDto>("At least one permission required");
                    #endregion
                    apiResponse = new APIsSuccsss<EntityRoleDto>("Role Created Successfully", response);
                }
                else
                {
                    #region Remove All Permission of current role
                    await _idbConnection.ExecuteAsync($@"DELETE FROM {DbTables.tblRolePermissions} WHERE RoleId = @RoleId", new { RoleId = Id }, _idbTransaction);
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
                    else
                        return new APIsError<EntityRoleDto>("At least one permission required");
                    #endregion
                    apiResponse = new APIsSuccsss<EntityRoleDto>("Role Updated Successfully");
                }
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

        public async Task<APIsResponse<List<EntityPermissionDto>>> GetRole(long roleId)
        {
            try
            {
                var sql = $@" 
                            SELECT p.Id,p.Name,p.Module,p.Description
                            FROM {DbTables.tblRole} r
                            INNER JOIN {DbTables.tblRolePermissions} rp ON r.Id = rp.RoleId
                            INNER JOIN {DbTables.tblPermissions} p ON p.Id = rp.PermissionId
                            WHERE r.Id = @Id;
                        ";
                var result = await _idbConnection.QueryAsync<EntityPermissionDto>(
                                sql,
                                new { Id = roleId },
                                transaction: _idbTransaction
                            );

                if (result == null || !result.Any())
                    return new APIsError<List<EntityPermissionDto>> (_validateMessages.NotExist);

                return new APIsSuccsss<List<EntityPermissionDto>>(_validateMessages.RetriveSuccess, result);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<EntityPermissionDto>>(ex.GetActualError()));
            }
        }
    }
}
