using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("PayBonusPunish")]
    public class PayBonusPunish
    {
        [Key]
        public int Id { get; set; }

        //public int? PayrollId { get; set; }
        //[ForeignKey("PayrollId")]
        //public Payroll? Payroll { get; set; }

        public int? BonusPunishId { get; set; }
        [ForeignKey("BonusPunishId")]
        public BonusPunish? BonusPunish { get; set; }
    }
}
