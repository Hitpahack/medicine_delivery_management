using Dapper;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Dtos.DataTables;
using RepMed.Dtos.RolePage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IRoleService : IDisposable 
    {
        Task<APIsResponse<EntityRoleDto>> AddEditRole(CreateRoleDto reqDto, long Id);
        Task<APIsResponse<EntityRoleDto>> AddEditPharmacyRole(CreateRoleDto reqDto, long Id);
        Task<APIsResponse<bool>> DeleteRole(long Id);
        Task<APIsResponse<GetRoleDto>> GetRolePermission(long roleId);
        Task<APIsResponse<Datatable<RolesPagingResponse >>> GetAllRoles(RolesPagingRequest reqDto);
        Task<APIsResponse<Datatable<RolesPagingResponse >>> GetAllPharmacyRoles(RolesPagingRequest reqDto);
        Task<APIsResponse<EntityRoleDto>> ChangeRoleStatus(long Id, bool status);
        Task<APIsResponse<List<EntityPermissionDto>>> GetAllPermissions();
        Task<APIsResponse<List<EntityPermissionDto>>> GetAllPharmacyPermissions();
    }
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public async Task<APIsResponse<EntityRoleDto>> AddEditRole(CreateRoleDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                if (Id == 0)
                {
                    #region Assign Dashboards to Role
                    if (reqDto.RoleName == "admin")
                        reqDto.PermissionIds.Add(6);
                    else if (reqDto.RoleName == "doctor")
                        reqDto.PermissionIds.Add(7);
                    else if (reqDto.RoleName == "pharmacy")
                        reqDto.PermissionIds.Add(8);
                    else
                        reqDto.PermissionIds.Add(9);
                    #endregion
                    #region Check RoleExist
                    if ((await IsRoleExist(reqDto.RoleName,reqDto.PharmacyId)))
                    {
                        return await Task.FromResult(new APIsError<EntityRoleDto>(_validateMessages.AlreadyExist));
                    }
                    #endregion
                    #region Add Role
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.IsAdminRole =true;
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
                    { nameof(EntityRoleDto.UpdatedAt), DateTime.Now },
                    { nameof(EntityRoleDto.RoleName), reqDto.RoleName},
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

        public async Task<APIsResponse<EntityRoleDto>> ChangeRoleStatus(long Id, bool status)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                EntityRoleDto entityRoleDto = _idbConnection.Update<EntityRoleDto>(_idbTransaction, DbTables.tblRole,
                   new Dictionary<string, object> {
                    { nameof(EntityRoleDto.IsActive), status},
                   }, $@" {nameof(EntityRoleDto.Id)}='{Id}' ", "RETURNING *");

                apiResponse = new APIsSuccsss<EntityRoleDto>("Role Status Updated Successfully", entityRoleDto);
                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<EntityRoleDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<bool>> DeleteRole(long Id)
        {
            try
            {
                APIsResponse<bool> apiResponse = default(APIsResponse<bool>);
                #region Check if role is assigned to any user
                string checkQuery = $@"SELECT UserId FROM {DbTables.tblUserRoles} WHERE RoleId = @RoleId;";
                int assignedUserCount = await _idbConnection.ExecuteScalarAsync<int>(checkQuery, new { RoleId = Id }, _idbTransaction);

                if (assignedUserCount > 0)
                {
                    return new APIsError<bool>("Cannot delete role. It is assigned to one or more users.");
                }
                #endregion
                #region Delete the role permissions
                string query = $@"DELETE FROM {DbTables.tblRolePermissions} WHERE RoleId = @RoleId;";
                int rows = await _idbConnection.ExecuteAsync(query, new { RoleId = Id }, _idbTransaction);
                #endregion

                #region Delete the role
                string deleteQuery = $@"DELETE FROM {DbTables.tblRole} WHERE Id = @RoleId;";
                int rowsAffected = await _idbConnection.ExecuteAsync(deleteQuery, new { RoleId = Id }, _idbTransaction);

                if (rowsAffected > 0)
                    apiResponse = new APIsSuccsss<bool>("Role deleted successfully.", true);
                else
                    apiResponse = new APIsSuccsss<bool>("Role not found.", true);
                #endregion
                return apiResponse;
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<bool>(ex.GetActualError()));
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
                string query = DbTables.tblPermissions.SelectAll($@"{nameof(Permission.IsAdmin)} = true and {nameof(Permission.Id)} not in (6,7,8,9)");
                var rolepermissions = await _idbConnection.QueryAsync<EntityPermissionDto>(query, transaction: _idbTransaction);
                return new APIsSuccsss<List<EntityPermissionDto>>(_validateMessages.RetriveSuccess, rolepermissions);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<EntityPermissionDto>>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<List<EntityPermissionDto>>> GetAllPharmacyPermissions()
        {
            try
            {
                string query = DbTables.tblPermissions.SelectAll($@"{nameof(Permission.IsAdmin)} = false");
                var rolepermissions = await _idbConnection.QueryAsync<EntityPermissionDto>(query, transaction: _idbTransaction);
                return new APIsSuccsss<List<EntityPermissionDto>>(_validateMessages.RetriveSuccess, rolepermissions);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<List<EntityPermissionDto>>(ex.GetActualError()));
            }
        }
        public async Task<APIsResponse<Datatable<RolesPagingResponse>>> GetAllRoles(RolesPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<RolesPagingResponse>> apiResponse = default;
                string orderBy;

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
                            SELECT p.Id,p.Route,p.Module
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

                if (permissions[0].Id == 0)
                    permissions = null;
                GetRoleDto result = new GetRoleDto
                {
                    Role = role,
                    Permissions = permissions
                };
                if (result == null)
                    return new APIsError<GetRoleDto>(_validateMessages.NotExist);
                else
                    return new APIsSuccsss<GetRoleDto>(_validateMessages.RetriveSuccess, result);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new APIsError<GetRoleDto>(ex.GetActualError()));
            }
        }

        public async Task<APIsResponse<EntityRoleDto>> AddEditPharmacyRole(CreateRoleDto reqDto, long Id)
        {
            try
            {
                APIsResponse<EntityRoleDto> apiResponse = default(APIsResponse<EntityRoleDto>);
                if (Id == 0)
                {
                    #region Assign Dashboards to Role
                    if (reqDto.RoleName == "admin")
                        reqDto.PermissionIds.Add(6);
                    else if (reqDto.RoleName == "doctor")
                        reqDto.PermissionIds.Add(7);
                    else if (reqDto.RoleName == "pharmacy")
                        reqDto.PermissionIds.Add(8);
                    else
                        reqDto.PermissionIds.Add(9);
                    #endregion

                    #region Check RoleExist
                    if ((await IsRoleExist(reqDto.RoleName, reqDto.PharmacyId)))
                    {
                        return await Task.FromResult(new APIsError<EntityRoleDto>(_validateMessages.AlreadyExist));
                    }
                    #endregion
                    #region Add Role
                    reqDto.CreatedAt = DateTime.Now;
                    reqDto.IsActive = true;
                    reqDto.IsAdminRole = false;
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
                    { nameof(EntityRoleDto.UpdatedAt), DateTime.Now },
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

        public async Task<APIsResponse<Datatable<RolesPagingResponse>>> GetAllPharmacyRoles(RolesPagingRequest reqDto)
        {
            try
            {
                APIsResponse<Datatable<RolesPagingResponse>> apiResponse = default;
                string orderBy;

                orderBy = reqDto.Columns[reqDto.Order[0].Column].Data + "|" + reqDto.Order[0].Dir;
                #region Get All Pharmacy 
                var parameters = new DynamicParameters();
                parameters.Add("page", reqDto.Page, DbType.Int32);
                parameters.Add("pageSize", reqDto.PageSize, DbType.Int32);
                parameters.Add("searchText", reqDto.SearchText ?? string.Empty, DbType.String);
                parameters.Add("pharmacyId", reqDto.PharmacyId, DbType.Int32);
                parameters.Add("statusFilter", reqDto.StatusFilter ?? string.Empty, DbType.String);
                parameters.Add("Order_by", orderBy, DbType.String);

                var result = (await _idbConnection.QueryAsync<RolesPagingResponse>(
                               sql: "GET_PHARMACY_ROLES_PAGED",
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
    }
}
