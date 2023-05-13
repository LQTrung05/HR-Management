using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models
{

    public class PositionModelView
    {
        public int Id { get; set; }

        public string? PositionName { get; set; }
        public string? Note { get; set; }
        public float? Coefficient { get; set; }
    }
}
