using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanDetailsAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found :(";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var model = await _planService.GetPlanForEditAsync(id, ct);
            if (model is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found :(";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPlanViewModel model, CancellationToken ct = default)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _planService.UpdatePlanAsync(model, ct);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id, CancellationToken ct = default)
        {
            var (success, error) = await _planService.ToggleActiveStatusAsync(id, ct);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] =
                success ? "Plan Status Changed" : error;

            return RedirectToAction(nameof(Index));
        }
    }
}