using GymManagement.BLL.ViewModels.Plans;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<List<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanDetailsAsync(int id, CancellationToken ct = default);
        Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken ct = default);
        Task<(bool Success, string? ErrorMessage)> UpdatePlanAsync(EditPlanViewModel model, CancellationToken ct = default);
        Task<(bool Success, string? ErrorMessage)> ToggleActiveStatusAsync(int id, CancellationToken ct = default);
    }
}
