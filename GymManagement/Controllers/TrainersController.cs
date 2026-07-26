using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerCreateViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Trainer added successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);
            if (!result.Success)
                return result.Kind == ResultKind.NotFound ? NotFound() : BadRequest(result.Error);

            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (!result.Success)
                return result.Kind == ResultKind.NotFound ? NotFound() : BadRequest(result.Error);

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainerEditViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);
            if (!result.Success)
            {
                if (result.Kind == ResultKind.NotFound) return NotFound();
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Trainer updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var result = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);
            if (!result.Success)
                return result.Kind == ResultKind.NotFound ? NotFound() : BadRequest(result.Error);

            return View(result.Value);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, ct);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Trainer deleted successfully." : result.Error;

            return RedirectToAction(nameof(Index));
        }
    }
}