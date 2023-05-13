using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models.Search
{

    public class SearchModelView:PageViewModel
    {
        public string? name { get; set; }

        public string? selectedNhanVien { get; set; }

        public float? Coefficient { get; set; }
        public string? Note { get; set; }        
    }
}
