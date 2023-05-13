using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models
{
    public class ProjectModelView
    {
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartDateStr { get; set; }
        public string? EndDateStr { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
        public string? StatusStr { get; set; }
        public string? Note { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? IsDeletedStr { get; set; }
    }
}
