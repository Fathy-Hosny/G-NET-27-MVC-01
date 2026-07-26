using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL;
using GymManagement.Models;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return _mapper.Map<List<PlanViewModel>>(plans);
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct: ct);
            return plan is null ? null : _mapper.Map<PlanViewModel>(plan);
        }

        public async Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct: ct);
            return plan is null ? null : _mapper.Map<EditPlanViewModel>(plan);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdatePlanAsync(EditPlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.PlanRepository.GetPlanWithMembershipsAsync(model.Id, ct);
            if (plan is null)
                return (false, "Plan Not Found");

            // Business rule: cannot update a plan with active memberships
            if (plan.Members is not null && plan.Members.Any(m => m.IsActive))
                return (false, "Cannot update a plan with active memberships");

            // Name is locked - never updated, even if the form is tampered with
            plan.Price = model.Price;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(ct);
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> ToggleActiveStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.PlanRepository.GetPlanWithMembershipsAsync(id, ct);
            if (plan is null)
                return (false, "Plan Not Found");

            // Only block deactivation, activation is always allowed
            if (plan.IsActive && plan.Members is not null && plan.Members.Any(m => m.IsActive))
                return (false, "Cannot deactivate a plan with active memberships");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(ct);
            return (true, null);
        }
    }
}