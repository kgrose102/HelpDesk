using Microsoft.AspNetCore.Mvc;
using ExercisesWebsite.Reports;
using HelpdeskWebsite.Reports;
namespace ExercisesWebsite.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public ReportController(IWebHostEnvironment env)
        {
            _env = env;
        }
        //[Route("api/helloreport")]
        //[HttpGet]
        //public IActionResult GetHelloReport()
        //{
        //    HelloReport hello = new();
        //    hello.GenerateReport(_env.WebRootPath);
        //    return Ok(new { msg = "Report Generated" });
        //}

        [Route("api/employeereport")]
        [HttpGet]
        public IActionResult GetEmployeeReport()
        {
            EmployeeReport hello = new();
            hello.GenerateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/callreport")]
        [HttpGet]
        public IActionResult GetCallReport()
        {
            CallReport hello = new();
            hello.GenerateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }
    }
}
