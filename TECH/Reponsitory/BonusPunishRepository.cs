using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IBonusPunishRepository : IRepository<BonusPunish>
    {
       
    }

    public class BonusPunishRepository : EFRepository<BonusPunish>, IBonusPunishRepository
    {
        public BonusPunishRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
