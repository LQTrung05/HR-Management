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

    public class BonusPunishModelView
    {
        public int Id { get; set; }
        public int? PayrollId { get; set; }
        public PayrollModelView? PayrollModelView { get; set; }
        public int? RewardType { get; set; }
        public string? RewardTypeStr { get; set; }
        public string? RewardTypeName { get; set; }
        public decimal? PenaltyBonus { get; set; }
        public string? PenaltyBonusStr { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Note { get; set; }
    }
}
