using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseStaticPageDto
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public string Content { get; set; }

        public string MetaTitle { get; set; }

        public string MetaContext { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
    public class EntityStaticPageDto: BaseStaticPageDto
    {
        public long Id { get; set; }
    }
}
