using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Services;
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
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
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

        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> States(long? countryid)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        APIsResponse<IEnumerable<SelectListItem>> countries;
                        if (countryid > 0)
                            countries = await masterService.GetStates(s => s.CountryId == countryid);
                        else
                            countries = await masterService.GetStates();

                        tran.Commit();

                        return countries;

                    }
                }
            }

        }

        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> Cities(long? stateid)
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        APIsResponse<IEnumerable<SelectListItem>> countries;    
                        if (stateid > 0)
                            countries = await masterService.GetCities(s => s.StateId == stateid);
                        else
                            countries = await masterService.GetCities();
                        
                        if(countries.IsSuccess)
                        tran.Commit();

                        return countries;

                    }
                }
            }

        }
        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> Roles()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        var countries = await masterService.GetRoles();
                        tran.Commit();
                        return countries;

                    }
                }
            }

        }
        protected async Task<APIsResponse<IEnumerable<SelectListItem>>> Products()
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    using (IMasterService masterService = new MasterService(db, tran))
                    {
                        var countries = await masterService.GetProducts();
                        tran.Commit();
                        return countries;

                    }
                }
            }

        }
    }
}
