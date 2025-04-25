using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class UserTokenLogDto
    {
        public long? UserID { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidTill { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long SuperAdminId { get; set; }
    }

    public class EntityUserTokenLogDto : UserTokenLogDto
    {
        public int Id { get; set; }
        
    }

    public class AddUserTokenLogDto : UserTokenLogDto
    {
        
       
    }
}
