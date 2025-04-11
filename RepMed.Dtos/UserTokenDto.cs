using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class UserTokenDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool? IsUsed { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
