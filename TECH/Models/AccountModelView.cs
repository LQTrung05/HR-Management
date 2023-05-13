using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models
{

    public class AccountModelView
    {
        public string EmployeeCode { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
    }
}
