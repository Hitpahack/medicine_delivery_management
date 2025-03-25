using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BaseCodeRequestDtos
    {
        
        public string Userid { get; set; }
        public DateTime ValidTo { get; set; }
        public string SecurityCode { get; set; }
        public bool? IsExpired { get; set; }
    }

    public class CodeRequestDtos : BaseCodeRequestDtos
    {
        public int Id { get; set; }
        
    }
}
