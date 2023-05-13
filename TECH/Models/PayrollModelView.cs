using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;
namespace TECH.Models
{

    public class PayrollModelView
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public EmployeeModelView? Employee { get; set; }
        public int? BonusPunishId { get; set; }
        public decimal? Basicsalary { get; set; }
        public string? BasicsalaryStr { get; set; }
        public decimal? SalaryKPI { get; set; }
        public string? SalaryKPIStr { get; set; }
        public List<BonusPunishModelView>? BonusPunish { get; set; }
        public PositionModelView? Position { get; set; }
        public string? Coefficient { get; set; }
        public string? TotalSalaryStr { get; set; }
        public decimal? TotalSalary { get; set; }
        public string? TotalPenaltyBonusStr { get; set; }
        public string? TotalBonusStr { get; set; }
        public string? TotalPunishStr { get; set; }
        public string? Note { get; set; }
    }
}
