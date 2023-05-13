using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models
{

    public class EmployeeModelView
    {
        public string? EmployeeCode { get; set; }
        public int? PositionCode { get; set; }
        public string? PositionCodeStr { get; set; }
        public int? RoomCode { get; set; }
        public string? RoomCodeStr { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? CMND { get; set; }
        public int? LevelAcademic { get; set; }
        public string? LevelAcademicStr { get; set; }
        public int? Gender { get; set; }
        public string? GenderStr { get; set; }
        public DateTime? YearOfBirth { get; set; }
        public string? YearOfBirthStr { get; set; }
        public string? Address { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? IsDeletedStr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDated { get; set; }
        public string? CreatedDatedStr { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDatedStr { get; set; }
        public DateTime? UpdatedDated { get; set; }
        public decimal? Basicsalary { get; set; }
        public string? TotalBonusStr { get; set; }
        public string? TotalPunishStr { get; set; }
        public string? TotalPenaltyBonusStr { get; set; }
        public float? Coefficient { get; set; }
    }
}
