export class BasicRoleDto {
    name: string;
    description: string;
}

export class EntityRoleDto extends BasicRoleDto {
    id: number;
    permission:UserRolePermissionDto[]|null;
}

export class EntityUserRoleDto {
    userId: string;
    roleId: number;
    userRolePermission :UserRolePermissionDto[]|null
}

export class RolePermissionsDto {
    id: string;
    permission: string;
    description: string;
    title: string;
    route: string;
    action: string;
    controller: string;
    parma: string;
    isApis: boolean;
    isWeb: boolean;
    isAdmin: boolean;
}

export class UserRolePermissionDto {
    id: string;
    roleId: number;
    permissionId: string;
    canAdd: boolean | null;
    canEdit: boolean | null;
    canDelete: boolean | null;
    canListing: boolean | null;
    canDetail: boolean | null;
    permission: RolePermissionsDto;
    role: EntityRoleDto;
}