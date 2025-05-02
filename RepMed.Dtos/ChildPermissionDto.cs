using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseChildPermissionDto
    {
        public long PermissionId { get; set; }

        public string Label { get; set; }

        public string Route { get; set; }
    }
    public class EntityChildPermissionDto: BaseChildPermissionDto
    {
        public long Id { get; set; }
    }
}
