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
        public int Id { get; set; }
    }

    public class EntityUserRoleDto
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

    }
    public class CreateRoleDto : BasicRoleDto
    {
        public List<long>? PermissionIds { get; set; }
    }

    public class GetRoleDto
    {

    }


}
