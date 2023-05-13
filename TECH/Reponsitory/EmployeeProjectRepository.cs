using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IEmployeeProjectRepository : IRepository<EmployeeProject>
    {
       
    }

    public class EmployeeProjectRepository : EFRepository<EmployeeProject>, IEmployeeProjectRepository
    {
        public EmployeeProjectRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
