/*
 * Module Title: DepartmentController.cs
 * Coder: Kenneth Rose
 * Purpose: REST access layers for the Department access
 * Date: Oct. 27, 2024
 */

using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]//default route. All routes must be unique
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                DepartmentViewModel viewmodel = new();
                List<DepartmentViewModel> allDeps = await viewmodel.GetAll();
                return Ok(allDeps);
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
