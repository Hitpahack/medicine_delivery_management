using RepMed.Data;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface ICMSServies : IDisposable
    {
        Task<APIsResponse<EntityStaticPageDto>> CreateStaticPage(BaseStaticPageDto dto);
    }
    public class CMSServies:BaseService, ICMSServies
    {
        public CMSServies(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public async Task<APIsResponse<EntityStaticPageDto>> CreateStaticPage(BaseStaticPageDto dto)
        {
            throw new NotImplementedException();
            //var page = new StaticPage
            //{
            //    Title = dto.Title,
            //    Slug = dto.Slug.ToLower().Replace(" ", "-"),
            //    Content = dto.Content,
            //    IsActive = dto.IsActive,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdatedDate = DateTime.UtcNow
            //};

            //// Save using Dapper or EF
            //await _db.InsertAsync(page); // Example method

            //return APIsResponse.Success("Static page added successfully.");
        }

    }
}
