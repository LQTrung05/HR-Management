using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string? EmployeeCode { get; set; }

        public int? PositionCode { get; set; }
        [ForeignKey("PositionCode")]
        public Position? Position { get; set; }

        public int? RoomCode { get; set; }
        [ForeignKey("RoomCode")]
        public Department? Department { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Avatar { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? FullName { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? SDT { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? CMND { get; set; }
       public int? LevelAcademic { get; set; }
       public int? Gender { get; set; }
       public DateTime? YearOfBirth { get; set; }
       [Column(TypeName = "nvarchar(500)")]
       public string? Address { get; set; }
        public bool? IsDeleted { get; set; } = false;
        [Column(TypeName = "nvarchar(500)")]
        public string? CreatedBy { get; set; }        
        public DateTime? CreatedDated { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDated { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Basicsalary { get; set; }

    }
}
