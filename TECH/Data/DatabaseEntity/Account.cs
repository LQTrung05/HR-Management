using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("Account")]
    public class Account
    {
        [Key]
        [Column(TypeName = "varchar(500)")]
        public string EmployeeCode { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string? UserName { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string? PassWord { get; set; }
    }
}
