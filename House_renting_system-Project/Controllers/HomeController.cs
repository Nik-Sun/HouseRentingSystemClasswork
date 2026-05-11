using House_renting_system_Project.Models;
using House_Renting_System_Project.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace House_renting_system_Project.Controllers
{
    public class HomeController : Controller
    {
		private readonly IStatisticsService statService;

		public HomeController(IStatisticsService statService)
		{
			this.statService = statService;
		}

		public IActionResult Index()
        {
			ViewBag.TotalRequests = statService.TotalRequests;
			return View();
        }

		public IActionResult Error(int statusCode)
		{
			if (statusCode == 401)
			{
				return View("Unauthorized");
			}

			return View("NotFound");
		}

		public IActionResult Crash()
		{
			throw new Exception("Test exception");
		}

		public IActionResult ServerError()
		{
			return View();
		}
	}
}
