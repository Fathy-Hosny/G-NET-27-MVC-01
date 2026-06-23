using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticViewModel;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using static System.Collections.Specialized.BitVector32;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnalyticService(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
            
        }
        public async Task<AnalyticViewModel> GetAnalyticsAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct : ct);
            var Trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct : ct);
            var memberships = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync(ct : ct);
            var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(ct : ct);

            return new AnalyticViewModel
            {
                TotalMembers = members.Count(),
                ActiveMembers = memberships.Count(m => m.IsActive),
                TotalTrainers = Trainers.Count(),
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.UtcNow),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.UtcNow),
            };
        }
    }
}
