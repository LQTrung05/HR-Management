using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
       
    }

    public class EmployeeRepository : EFRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
