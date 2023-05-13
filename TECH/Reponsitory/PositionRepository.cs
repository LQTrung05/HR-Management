using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IPositionRepository : IRepository<Position>
    {
       
    }

    public class PositionRepository : EFRepository<Position>, IPositionRepository
    {
        public PositionRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
