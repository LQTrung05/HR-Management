using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TECH.Data.DatabaseEntity
{
    public class DataBaseEntityContext : DbContext
    {
        public DataBaseEntityContext(DbContextOptions<DataBaseEntityContext> options) : base(options) { }

        public DbSet<Account> Accounts { set; get; }
        public DbSet<BonusPunish> BonusPunishs { set; get; }
        public DbSet<Department> Departments { set; get; }
        public DbSet<Employee> Employees { set; get; }
        public DbSet<EmployeeProject> EmployeeProjects { set; get; }
        //public DbSet<Payroll> Payrolls { set; get; }
        //public DbSet<PayBonusPunish> PayBonusPunishs { set; get; }
        public DbSet<Position> Positions { set; get; }
        public DbSet<Project> Projects { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }
    }
}
