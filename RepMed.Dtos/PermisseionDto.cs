using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePermissionDto
    {
        public string Module { get; set; }
        public string Route { get; set; }
    }
    public class EntityPermissionDto: BasePermissionDto
    {
        public long Id { get; set; }
       
    }

}
