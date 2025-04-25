using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BaseUserSecurityCodeDto
    {
        public string SecurityCode { get; set; }

        public long UserId { get; set; }

        public DateTime ValidTo { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public bool IsExpired { get; set; }



    }

    public class UserSecurityCodeDto : BaseUserSecurityCodeDto
    {
        public long Id { get; set; }


    }
}
