using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.UsersPage
{
    public class UsersPagingResponse: SP_BASE_RESPONSE
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public bool IsLocked { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
