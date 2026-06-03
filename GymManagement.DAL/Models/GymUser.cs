using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models.Enums;

namespace GymManagement.DAL.Models
{
    public class GymUser : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
     
    }
}
