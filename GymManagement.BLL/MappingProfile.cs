using AutoMapper;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.BLL.ViewModels.Trainer;
using GymManagement.DAL.Models;
using GymManagement.Models;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapPlan();
            MapSession();
            MapTrainer(); // 1. أضفنا استدعاء الـ MapTrainer هنا
        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
            {
                City = src.City,
                Street = src.Street,
                BuildingNumber = src.BuildingNumber
            }))
            .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => new HealthRecord()
            {
                Height = src.HealthRecordViewModel.Height,
                Weight = src.HealthRecordViewModel.Weight,
                BloodType = src.HealthRecordViewModel.BloodType,
                Note = src.HealthRecordViewModel.Note
            }));

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null
                    ? $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"
                    : string.Empty))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src =>
                    src.Plans == null ? "No Plan" :
                    src.Plans.FirstOrDefault(p => p.IsActive) == null ? "No Active Plan" :
                    src.Plans.FirstOrDefault(p => p.IsActive).Plan == null ? "No Active Plan" :
                    src.Plans.FirstOrDefault(p => p.IsActive).Plan.Name))
                .ForMember(dest => dest.MembershipStartDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.MembershipEndDate, opt => opt.MapFrom(src =>
                    src.Plans != null && src.Plans.FirstOrDefault(p => p.IsActive) != null
                        ? src.Plans.FirstOrDefault(p => p.IsActive).EndDate.ToString("yyyy-MM-dd")
                        : "N/A"));

            CreateMap<HealthRecord, HealthRecordViewModel>();

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, EditPlanViewModel>();
        }

        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(D => D.TrainerName, O => O.MapFrom(S => S.Trainer != null ? $"{S.Trainer.Name}" : "No Trainer"))
                .ForMember(D => D.CategoryName, O => O.MapFrom(S => S.Category != null ? S.Category.CategoryName : "No Category"));
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>();
        }

        // 2. أضفنا الدالة الخاصة برسم الخرائط للـ Trainer
        private void MapTrainer()
        {
            // من Trainer إلى TrainerViewModel (تستخدم في عرض القائمة)
            CreateMap<Trainer, TrainerViewModel>();

            // من Trainer إلى TrainerDetailsViewModel (تستخدم في تفاصيل المدرب)
            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null
                    ? $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"
                    : "No Address"));

            // من Trainer إلى TrainerEditViewModel (تستخدم لصفحة التعديل)
            CreateMap<Trainer, TrainerEditViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNumber : 0))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : string.Empty))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty));

            // من TrainerCreateViewModel إلى Trainer (تستخدم عند إنشاء مدرب جديد)
            CreateMap<TrainerCreateViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
        }
    }
}