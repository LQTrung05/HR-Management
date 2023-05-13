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
    public class PayBonusPunishModelView
    {
        public int Id { get; set; }
        public int? PayrollId { get; set; }
        public PayrollModelView? Payroll { get; set; }

        public int? BonusPunishId { get; set; }
        public BonusPunishModelView? BonusPunish { get; set; }
    }
}
