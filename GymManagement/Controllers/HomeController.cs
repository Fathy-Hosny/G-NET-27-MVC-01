using System.Diagnostics;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analyticService;

        public HomeController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var analytics = await _analyticService.GetAnalyticsAsync(ct);

            return View(analytics);
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
