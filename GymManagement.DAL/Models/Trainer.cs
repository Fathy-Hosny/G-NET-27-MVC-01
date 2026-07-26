using GymManagement.DAL.Models.Enums;

namespace GymManagement.DAL.Models
{

    public class Trainer : GymUser
    {
        public Specialties Specialty{ get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
