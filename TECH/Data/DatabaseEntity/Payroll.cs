﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("Payroll")]
    public class Payroll
    {
        [Key]
        public int Id { get; set; }

        public string? EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public Employee? Employee { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Basicsalary { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? SalaryKPI { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? TotalSalary { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }
    }
}
