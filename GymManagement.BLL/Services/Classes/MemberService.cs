using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
           var members = await _memberRepository.GetAllAsync(ct: ct);
            // Mapping from Member to MemberViewModel should be done here
            //var memberViewModels = new List<MemberViewModel>();

            //foreach (var member in members) {

            //    memberViewModels.Add(new MemberViewModel
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Photo = member.Photo,
            //        Gender = member.Gender.ToString()

            //    });
            //}

            var memberViewModels = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString()
            });
            return memberViewModels;
        }
        public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

       

        public Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

      

       

     

     
    }
}
