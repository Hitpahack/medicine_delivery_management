using RepMed.Core;
using RepMed.Dtos;
using RepMed.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.BaseApis
{
    public class BaseMastersController : BaseAPIsController
    {
        public BaseMastersController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> Countries()
        {
            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        var countries = await masterService.GetCounrties();
                        tran.Commit();

                        return countries;

                    }
                }
            }

        }

        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> States()
        {
            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        var countries = await masterService.GetStates();
                        tran.Commit();

                        return countries;

                    }
                }
            }

        }

        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> Cities()
        {
            using (var db = new NpgsqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        var countries = await masterService.GetCities();
                        tran.Commit();

                        return countries;

                    }
                }
            }

        }
    }
}
