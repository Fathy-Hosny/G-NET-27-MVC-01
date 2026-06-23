using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropdownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropdownAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int SessionId, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default);

        Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default);

    }
}
