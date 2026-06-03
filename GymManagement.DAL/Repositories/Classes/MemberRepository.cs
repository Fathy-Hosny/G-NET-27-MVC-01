using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;

namespace GymManagement.DAL.Repositories.Classes
{
    internal class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbcontext context) : base(context)
        {
        }
    }
}
