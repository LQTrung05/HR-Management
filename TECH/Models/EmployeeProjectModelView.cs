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

    public class EmployeeProjectModelView
    {
        public int Id { get; set; }

        public string? EmployeeCode { get; set; }
        public EmployeeModelView? Employee { get; set; }

        public int? ProjectCode { get; set; }
    }
}
