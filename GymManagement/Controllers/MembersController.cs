using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberService, IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }
        [HttpGet]
        public async Task<IActionResult> Picture(int id , CancellationToken ct = default)
        {
            var members = await _memberService.GetMemberDetailsAsync(id, ct);
            if(members is null || string.IsNullOrWhiteSpace(members.Photo))
            {
                return NotFound();
            }   
            var result = _attachmentService.GetFile("MemberPicture" , members.Photo);
            if(result is null)
            {
                return NotFound();
            }
            return File(result.Value.fileStream , result.Value.contentType);
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.CreateMemberAsync(model, ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Member created successfully!";
                }
                else
                {
                   TempData["ErrorMessage"] = "Failed to create member :( Please try again.";
                }
                return RedirectToAction(nameof(Index));


            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberDetailsAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found.";
                return RedirectToAction("Index");
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "HealthRecord Record Not Found.";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found :(";
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id,MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(id, model, ct);


                if (result)
                {
                    TempData["SuccessMessage"] = "Member updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update member :( Please try again.";
                }
                return RedirectToAction("Index");


            }

            return View(model);
        }



        [HttpGet]

        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberDetailsAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found :(";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member :( Please try again.";
            }
            return RedirectToAction("Index");

        }

    }
}
