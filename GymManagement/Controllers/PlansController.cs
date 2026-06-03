using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        //public readonly GymDbcontext _context = new GymDbcontext();
        private readonly IGenericRepository<Plan> _planRepository; //null
        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            //var plans = await _context.Plans.ToListAsync();
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct: ct);
            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }



    }
}
