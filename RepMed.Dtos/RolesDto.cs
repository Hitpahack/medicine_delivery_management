using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BasicRoleDto 
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EntityRoleDto : BasicRoleDto
    {
        public int Id { get; set; }
        //public IList<UserRolePermissionDto> UserRolePermission { get; set; }
        //public virtual ICollection<UserRoles> UserRoles { get; set; }

    }

    public class EntityUserRoleDto 
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

        //public virtual Role Role { get; set; }
        //public virtual User User { get; set; }

    }

    public class RolePermissionsDto
    {
        public RolePermissionsDto()
        {
            //UserRolePermission = new HashSet<UserRolePermission>();
        }

        public long Id { get; set; }
        public string Permission { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Parma { get; set; }
        public bool IsApis { get; set; }
        public bool IsWeb { get; set; }
        public bool IsAdmin { get; set; }

        //public virtual ICollection<UserRolePermission> UserRolePermission { get; set; }
    }

    public partial class UserRolePermissionDto
    {
        public long Id { get; set; }
        public int RoleId { get; set; }
        public long PermissionId { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanListing { get; set; }
        public bool? CanDetail { get; set; }

        public virtual RolePermissionsDto Permission { get; set; }
        public virtual EntityRoleDto Role { get; set; }
    }
}
