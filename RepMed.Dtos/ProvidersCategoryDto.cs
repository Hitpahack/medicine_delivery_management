using System;
using System.Collections.Generic;
using System.Text;
using static RepMed.Core.Enums;

namespace RepMed.Dtos
{
    public class BaseProvidersCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProviderType? ProfeType { get; set; }
        public Guid? CreatedBy { get; set; }
        public bool? Isactive { get; set; }
       
    }
    public class ProvidersCategoryDto : BaseProvidersCategoryDto
    {
        public int Id { get; set; }
        public string Picture { get; set; }

    }

    public class AddProvidersCategoryDto : BaseProvidersCategoryDto
    {

    }

    public class UpdateProvidersCategoryDto : BaseProvidersCategoryDto
    {
        public bool? Isdelete { get; set; }
    }



}
