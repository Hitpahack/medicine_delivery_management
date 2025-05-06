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
        public string Label { get; set; }
        public bool? IsAdmin { get; set; }

    }
    public class EntityPermissionDto : BasePermissionDto
    {
        public long Id { get; set; }
    }
    public class PermissionJoinDto
    {
        public int ParentId { get; set; }
        public string ParentModule { get; set; }
        public string ParentRoute { get; set; }
        public string ParentLabel { get; set; }
        public string ChildRoute { get; set; }
        public string ChildLabel { get; set; }
    }
    public class PermissionChildDto
    {
        public string Label { get; set; }
        public string Route { get; set; }
    }
    public class PermissionParentDto
    {
        public string Label { get; set; } // Parent Label
        public string Route { get; set; } // Parent Label
        public List<PermissionChildDto> Children { get; set; } = new();
    }


}
