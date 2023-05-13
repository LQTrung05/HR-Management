using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("EmployeeProject")]
    public class EmployeeProject
    {
        [Key]
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public Employee? Employee { get; set; }
        public int? ProjectCode { get; set; }
        [ForeignKey("ProjectCode")]
        public Project? Project { get; set; }
    }
}
