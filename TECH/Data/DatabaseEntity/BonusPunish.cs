using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("BonusPunish")]
    public class BonusPunish
    {
        [Key]
        public int Id { get; set; }
        //public int? PayrollId { get; set; }
        //[ForeignKey("PayrollId")]
        //public Payroll? Payroll { get; set; }
        public int? RewardType { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? RewardTypeName { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? PenaltyBonus { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }
        public string? EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public Employee? Employee { get; set; }
    }
}
