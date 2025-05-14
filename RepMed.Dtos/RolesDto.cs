using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BasicRoleDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAdminRole { get; set; }
    }

    public class EntityRoleDto : BasicRoleDto
    {
        public long Id { get; set; }
    }

    public class EntityUserRoleDto
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

    }
    public class CreateRoleDto : BasicRoleDto
    {
        public List<long> PermissionIds { get; set; }
        public long? PharmacyId { get; set; }
    }

    public class GetRoleDto
    {
        public EntityRoleDto Role { get; set; }
        public List<EntityPermissionDto> Permissions { get; set; }
    }
}
