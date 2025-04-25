using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BasicRoleDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
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
    }

    public class EditRoleDto
    {
        public List<long> PermissionIds { get; set; }

    }


}
