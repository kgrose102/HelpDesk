using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]//default route. All routes must be unique
    [ApiController]
    public class CallController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                CallViewModel viewmodel = new();
                List<CallViewModel> allDeps = await viewmodel.GetAll();
                return Ok(allDeps);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(CallViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Call " + viewmodel.EmployeeName + " updated!" }),
                    -1 => Ok(new { msg = "Employee " + viewmodel.EmployeeName + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.EmployeeName + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.EmployeeName + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CallViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Call " + viewmodel.EmployeeName + " added!" })
                : Ok(new { msg = "Call " + viewmodel.EmployeeName + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

    }
}
