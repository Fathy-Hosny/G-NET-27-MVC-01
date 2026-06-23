using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
     
        private readonly IMapper _mapper;   
        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate) return Result.ValidationFailed("End date must be after start date");
            if (model.StartDate <= DateTime.Now) return Result.ValidationFailed("Start date must be in the future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.ValidationFailed("Capacity must be between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null) return Result.NotFound($"Trainer with ID {model.TrainerId} not found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category == null) return Result.NotFound($"Category with ID {model.CategoryId} not found");

            var isValid =  Enum.TryParse<Specialties>(category.CategoryName, out var CategorySpecialty);
            if (!isValid|| trainer.Specialty != CategorySpecialty) return Result.ValidationFailed("Can not create session with trainer Because of specialty mismatch");


          var session =  _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);

            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.fail("Failed to create session!");
        }

     

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategoryAsync(ct);

            if (sessions == null || !sessions.Any()) return null;
            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropdownAsync(CancellationToken ct = default)
        {
           var trainers =await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
           
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropdownAsync(CancellationToken ct)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);

        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryByIdAsync(SessionId, ct);
            if (session == null) return Result<SessionViewModel>.NotFound($"Session with ID {SessionId} not found");

            var mappedSession = _mapper.Map<SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return Result<SessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
           var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

            if (session is null) return Result<UpdateSessionViewModel>.NotFound($"Session with ID {sessionId} not found");
            if(session.StartDate <= DateTime.Now) return Result<UpdateSessionViewModel>.fail("Cannot update a session that has already started or completed!");
            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if(bookedSlotsCount > 0) return Result<UpdateSessionViewModel>.fail("Cannot update a session that has already been booked!");
            var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {

            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

            if (session is null) return Result.NotFound($"Session with ID {sessionId} not found");
            if (session.StartDate <= DateTime.Now) return Result.fail("Cannot update a session that has already started or completed!");

            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookedSlotsCount > 0) return Result.fail("Cannot update a session that has already been booked!");

            if(model.StartDate >= model.EndDate) return Result.ValidationFailed("End date must be After Start date");
            if(model.StartDate <= DateTime.Now) return Result.ValidationFailed("Start date must be in the future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null) return Result.NotFound($"Trainer with ID {model.TrainerId} not found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
            if (category == null) return Result.NotFound($"Category with ID {session.CategoryId} not found");

            var isValid = Enum.TryParse<Specialties>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.ValidationFailed("Can not create session with trainer Because of specialty mismatch");


            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.Description = model.Description;
            session.TrainerId = model.TrainerId;
            session.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Session>().Update(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);

            return count > 0 ? Result.Ok() : Result.fail("Failed to Update Session!");

        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);

            if (session is null) return Result.NotFound($"Session with ID {sessionId} not found");

            if (session.EndDate >= DateTime.Now)
                return Result.fail("Cannot Delete a session that has already started or completed!");


            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookedSlotsCount > 0) return Result.fail("Cannot Delete a session that has already been booked!");

            _unitOfWork.GetRepository<Session>().Delete(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);

            return count > 0 ? Result.Ok() : Result.fail("Failed to Delete Session!");

        }
    }
}
