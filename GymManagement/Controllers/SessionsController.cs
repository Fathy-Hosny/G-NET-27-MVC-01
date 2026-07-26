using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            ViewBag.trainers = new SelectList(await _sessionService.GetAllTrainersForDropdownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetAllCategoriesForDropdownAsync(ct), "Id", "CategoryName");
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create(CreateSessionViewModel Model, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.CreateSessionAsync(Model, ct);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Session created successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Error;
                }
                return RedirectToAction("Index");
            }
            return View(Model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (result.Success)
                return View(result.Value);

            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.Success)
            {
            ViewBag.trainers = new SelectList(await _sessionService.GetAllTrainersForDropdownAsync(ct), "Id", "Name");

                return View(result.Value);
            }


            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel Model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(Model);


            var result = await _sessionService.UpdateSessionAsync(id, Model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.trainers = new SelectList(await _sessionService.GetAllTrainersForDropdownAsync(ct), "Id", "Name");

                TempData["ErrorMessage"] = result.Error;
                return View(Model);
            }

        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (result.Success)
           return View(result.Value);
            
             TempData["ErrorMessage"] = result.Error;
            
            return RedirectToAction("Index");
        }
        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }
            return RedirectToAction("Index");
        }

    }
}
