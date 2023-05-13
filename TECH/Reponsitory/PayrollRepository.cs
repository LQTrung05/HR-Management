using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IPayrollRepository : IRepository<Payroll>
    {
       
    }

    public class PayrollRepository : EFRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
