using GymManagement.BLL.ViewModels.AnalyticViewModel;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAnalyticService
    {
        Task<AnalyticViewModel> GetAnalyticsAsync(CancellationToken ct = default);
    }
}