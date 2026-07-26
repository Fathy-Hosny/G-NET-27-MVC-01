using System;
using System.Collections.Generic;
using System.Text;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        //Get All Members
        //Get Member By Id
        //Add new Member
        //Update Member
        //Delete Member
        //Get Member's Health Record
        //Get Member's to Update 
       Task<IEnumerable<MemberViewModel>> GetAllMembersAsync( CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync (int memberId, CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
