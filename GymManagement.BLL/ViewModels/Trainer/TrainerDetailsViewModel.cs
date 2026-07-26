using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models.Enums;

namespace GymManagement.BLL.ViewModels.Trainer
{
    public class TrainerDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Specialties Specialty { get; set; }
        public string Address { get; set; }
    }
}
