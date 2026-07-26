using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainer;
using GymManagement.DAL;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<Result<TrainerDetailsViewModel>> GetTrainerDetailsByIdAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)
                return Result<TrainerDetailsViewModel>.NotFound($"Trainer with ID {trainerId} not found");

            var mapped = _mapper.Map<TrainerDetailsViewModel>(trainer);
            return Result<TrainerDetailsViewModel>.Ok(mapped);
        }

        public async Task<Result<TrainerEditViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)
                return Result<TrainerEditViewModel>.NotFound($"Trainer with ID {trainerId} not found");

            var mapped = _mapper.Map<TrainerEditViewModel>(trainer);
            return Result<TrainerEditViewModel>.Ok(mapped);
        }

        public async Task<Result> CreateTrainerAsync(TrainerCreateViewModel model, CancellationToken ct = default)
        {
            var uniquenessValidation = await ValidateUniqueEmailAndPhoneAsync(model.Email, model.Phone, null, ct);
            if (!uniquenessValidation.Success) return uniquenessValidation;

            var trainer = _mapper.Map<Trainer>(model);
            _unitOfWork.GetRepository<Trainer>().Add(trainer);

            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.fail("Failed to create trainer!");
        }

        public async Task<Result> UpdateTrainerAsync(int trainerId, TrainerEditViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)
                return Result.NotFound($"Trainer with ID {trainerId} not found");

            var uniquenessValidation = await ValidateUniqueEmailAndPhoneAsync(model.Email, model.Phone, trainerId, ct);
            if (!uniquenessValidation.Success) return uniquenessValidation;

      
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;

      
            if (trainer.Address is null)
            {
                trainer.Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                };
            }
            else
            {
                trainer.Address.BuildingNumber = model.BuildingNumber;
                trainer.Address.Street = model.Street;
                trainer.Address.City = model.City;
            }

            trainer.Specialty = model.Specialty;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var count = await _unitOfWork.SaveChangesAsync(ct);

            return count > 0 ? Result.Ok() : Result.fail($"Failed to update trainer with ID {trainerId}!");
        }
        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)
                return Result.NotFound($"Trainer with ID {trainerId} not found");

            var hasScheduledSessions = await _unitOfWork.TrainerRepository.HasScheduledSessionsAsync(trainerId, ct);
            if (hasScheduledSessions)
                return Result.fail("Cannot delete this trainer because they have scheduled sessions!");

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var count = await _unitOfWork.SaveChangesAsync(ct);

            return count > 0 ? Result.Ok() : Result.fail($"Failed to delete trainer with ID {trainerId}!");
        }

        #region Private Validation Helpers

        private async Task<Result> ValidateUniqueEmailAndPhoneAsync(string email, string phone, int? excludeId, CancellationToken ct)
        {
            if (await _unitOfWork.TrainerRepository.IsEmailTakenAsync(email, excludeId, ct))
                return Result.ValidationFailed("This email is already used by another trainer.");

            if (await _unitOfWork.TrainerRepository.IsPhoneTakenAsync(phone, excludeId, ct))
                return Result.ValidationFailed("This phone number is already used by another trainer.");

            return Result.Ok();
        }

      

        #endregion
    }
}

