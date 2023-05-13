using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IDepartmentRepository : IRepository<Department>
    {
       
    }

    public class DepartmentRepository : EFRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
