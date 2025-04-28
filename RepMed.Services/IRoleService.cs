using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.PharmacyPage;
using RepMed.Dtos.RolePage;
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
        Task<APIsResponse<bool>> DeleteRole(long Id);
        Task<APIsResponse<GetRoleDto>> GetRolePermission(long roleId);
        Task<APIsResponse<Datatable<RolesPagingResponse >>> GetAllRoles(RolesPagingRequest reqDto);
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
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.IsActive = true;
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
                    #region Update Role
                    EntityRoleDto entityRoleDto = _idbConnection.Update<EntityRoleDto>(_idbTransaction, DbTables.tblRole,
                    new Dictionary<string, object> {
                    { nameof(EntityRoleDto.UpdatedAt), DateTime.UtcNow },
                    { nameof(EntityRoleDto.IsActive), reqDto.IsActive},
                    { nameof(EntityRoleDto.Description), reqDto.Description},
                    }, $@" {nameof(EntityRoleDto.Id)}='{Id}' ", "RETURNING *");
                    #endregion 

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

        public Task<APIsResponse<bool>> DeleteRole(long Id)
        {
            throw new NotImplementedException();
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

        public async Task<APIsResponse<Datatable<RolesPagingResponse>>> GetAllRoles(RolesPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<RolesPagingResponse>> apiResponse = default;
                string orderBy;
                if (reqDto.Order[0].Column == 0)
                    orderBy ="Id|desc";
                else
                    orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<RolesPagingResponse>(
                               sql: "GET_ROLES_PAGED",
                               param: parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: _idbTransaction
                )).ToList();
                #endregion
                var totalRecords = result.FirstOrDefault()?.TotalCount ?? 0;
                var output = new Datatable<RolesPagingResponse>(result, reqDto.Draw, totalRecords, totalRecords);
                if (result.Any())
                    return await Task.FromResult(new APIsSuccsss<Datatable<RolesPagingResponse>>(_validateMessages.RetriveSuccess, output));
                else
                    return await Task.FromResult(new APIsSuccsss<Datatable<RolesPagingResponse>>(_validateMessages.NotExist));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<Datatable<RolesPagingResponse>>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<GetRoleDto>> GetRolePermission(long roleId)
        {
            try
            {
                var sqlroles = $@" SELECT r.Id,r.RoleName,r.Description,r.IsActive
                               FROM {DbTables.tblRole} r WHERE r.Id =@Id";
                var role = await _idbConnection.QueryFirstOrDefaultAsync<EntityRoleDto>(
                               sqlroles,
                               new { Id = roleId },
                               transaction: _idbTransaction
                           );

                var sqlpermissions = $@" 
                            SELECT p.Id,p.Name,p.Module
                            FROM {DbTables.tblRole} r
                            LEFT JOIN {DbTables.tblRolePermissions} rp ON r.Id = rp.RoleId
                            LEFT JOIN {DbTables.tblPermissions} p ON p.Id = rp.PermissionId
                            WHERE r.Id = @Id;
                        ";
                var permissions = (await _idbConnection.QueryAsync<EntityPermissionDto>(
                                sqlpermissions,
                                new { Id = roleId },
                                transaction: _idbTransaction
                            )).ToList();

                GetRoleDto result = new GetRoleDto
                {
                    Role = role,
                    Permissions = permissions
                };
                if (result == null)
                    return new APIsError<GetRoleDto> (_validateMessages.NotExist);
                else
                    return new APIsSuccsss<GetRoleDto>(_validateMessages.RetriveSuccess, result);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<GetRoleDto>(ex.GetActualError()));
            }
        }
    }
}
