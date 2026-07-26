using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.Trainer;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

        Task<Result<TrainerDetailsViewModel>> GetTrainerDetailsByIdAsync(int trainerId, CancellationToken ct = default);

        Task<Result<TrainerEditViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default);

        Task<Result> CreateTrainerAsync(TrainerCreateViewModel model, CancellationToken ct = default);

        Task<Result> UpdateTrainerAsync(int trainerId, TrainerEditViewModel model, CancellationToken ct = default);

        Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}
