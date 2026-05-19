using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        public readonly GymDbcontext _context = new GymDbcontext();
        public async Task<IActionResult> Index()
        {
            var plans = await _context.Plans.ToListAsync();
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            var plan = _context.Plans.FirstOrDefault(p => p.Id == id);
            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }



    }
}
