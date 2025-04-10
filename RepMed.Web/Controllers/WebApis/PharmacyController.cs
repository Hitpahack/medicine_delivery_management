using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Services;
using RepMed.Web.Controllers.BaseApis;
using System.Threading.Tasks;

namespace RepMed.Web.Controllers.WebApis
{

    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/v1/admin/pharmacy")]
    public class PharmacyController : BaseAccountsController
    {
        public PharmacyController(IOptions<AppSettings> appSettings) : base(appSettings)
        {

        }

        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] PharmacyDto reqDto)  
        {
            using (var db = new MySqlConnection(_appSettings.ConnectionString))
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    using (IPharmacyService pharmacyService = new PharmacyService(db, tran))
                    {
                        var pharmacy = await pharmacyService.AddUpdatePharmacy(reqDto.Pharmacy , 0);
                        if (!pharmacy.IsSuccess)
                        {
                            tran.Rollback();
                            return null;
                        }
                        reqDto.PharmacyBankDetails.PharmacyId = pharmacy.Data.Id;
                        var pharmacybank = await pharmacyService.AddUpdatePharmacyBankDetails(reqDto.PharmacyBankDetails ,0);
                        if (!pharmacybank.IsSuccess)
                        {
                            tran.Rollback();
                            return null;
                        }
                        using (IPersonService personService = new PersonService(db, tran))
                        {
                            var user = await base.AddUser(reqDto.User);
                            if (!user.IsSuccess)
                            {
                                tran.Rollback();
                                return null;
                            }
                        }

                    }

                    tran.Commit();
                    return null;
                }

            }
        }
    }
}
