using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class EntityPermissionDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Module { get; set; }

        public string Description { get; set; }
    }
}
