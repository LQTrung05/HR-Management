using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Models.Search
{

    public class EmployeeModelViewSearch : PageViewModel
    {
        public string? EmployeeCode { get; set; }
        public int? PositionCode { get; set; }
        public int? Status { get; set; }
        public int? RoomCode { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public bool? IsSearch { get; set; }
    }
}
