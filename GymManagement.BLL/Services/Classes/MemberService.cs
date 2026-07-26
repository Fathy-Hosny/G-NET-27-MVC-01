using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
     private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;
        public MemberService(
         IUnitOfWork UnitOfWork,
         IMapper mapper
            , IAttachmentService attachmentService
            )
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {

           var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            #region Mappingreturn members;
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

            //var memberViewModels = members.Select(member => new MemberViewModel()
            //{
            //    Id = member.Id,
            //    Name = member.Name,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Photo = member.Photo,
            //    Gender = member.Gender.ToString()
            //});
            #endregion
            var result = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return result;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //Check Email
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            //Check Phone           _
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || phoneExists) return false;
            #region Mapping


            //Casting [Mapping] from CreateMemberViewModel to Member
            //var member = new Member()
            //{
            //    Name = model.Name,
            //    Email = model.Email,
            //    Phone = model.Phone,
            //    Gender = model.Gender,
            //    Address = new Address()
            //    {
            //        BuildingNumber = model.BuildingNumber,
            //        Street = model.Street,
            //        City = model.City,
            //    },
            //    HealthRecord = new HealthRecord()
            //    {
            //        Height = model.HealthRecordViewModel.Height,
            //        Weight = model.HealthRecordViewModel.Weight,
            //        BloodType = model.HealthRecordViewModel.BloodType,
            //        Note = model.HealthRecordViewModel.Note,
            //    }



            //    };
            //Add to Database

            #endregion
            var fileName = await _attachmentService.UploadFileAsync(model.PhotoFile.OpenReadStream(), "MemberPicture", model.PhotoFile.FileName, ct);
            if(string.IsNullOrWhiteSpace(fileName)) return false;
            var member =  _mapper.Map<Member>(model);
            member.Photo = fileName; // Set the uploaded file name
            _unitOfWork.GetRepository<Member>().Add(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            if(count > 0) return true;
            else {  return false; }
        }



        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return null;
         //var model = new MemberViewModel()
         //{
             
         //    Name = member.Name,
         //    Email = member.Email,
         //    Phone = member.Phone,
         //    Photo = member.Photo,
         //    Gender = member.Gender.ToString(),
         //    DateOfBirth = member.DateOfBirth.ToString(),
         //    Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
         //};
      var model =   _mapper.Map<MemberViewModel>(member);

            var ActiveMemberShip= await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);
            if(ActiveMemberShip != null)
            {
                model.Phone = ActiveMemberShip.Plan.Name ;
                model.MembershipStartDate = ActiveMemberShip.CreatedAt.ToString();
                model.MembershipEndDate = ActiveMemberShip.EndDate.ToString();
            }
            return model;
        }
        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
       var healthRecord   = await  _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct); 
            if (healthRecord == null) return null;
            //var model = new HealthRecordViewModel()
            //{
            //    Height = healthRecord.Height,
            //    Weight = healthRecord.Weight,
            //    BloodType = healthRecord.BloodType,
            //    Note = healthRecord.Note
            //};

            var model = _mapper.Map<HealthRecordViewModel>(healthRecord);
            return model;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return null;
            //var model = new MemberToUpdateViewModel()
            //{

            //    Name = member.Name,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Photo = member.Photo,
            //    City = member.Address.City,
            //    Street = member.Address.Street,
            //    BuildingNumber = member.Address.BuildingNumber  
            //};

            var model = _mapper.Map<MemberToUpdateViewModel>(member);
            return model;
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;
            //Check Email
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            //Check Phone
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            if (emailExists || phoneExists) return false;

            // Update member properties

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;

            _unitOfWork.GetRepository<Member>().Update(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;
        }
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;
            var hasFutureSesssions = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);
            if (hasFutureSesssions) return false;    
         
             _unitOfWork.GetRepository<Member>().Delete(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;
        }

      

       

     

     
    }
}
