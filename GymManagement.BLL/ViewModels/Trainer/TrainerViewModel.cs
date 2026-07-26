using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models.Enums;

namespace GymManagement.BLL.ViewModels.Trainer
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Specialties Specialty { get; set; }
    }
}
