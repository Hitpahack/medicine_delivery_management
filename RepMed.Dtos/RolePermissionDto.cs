using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseRolePermissionDto
    {
        public long RoleId { get; set; }

        public long PermissionId { get; set; }
    }
    public class EntityRolePermissionDto: BaseRolePermissionDto
    {
        public long Id { get; set; }

    }
}
