using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{

    [Table("Department")]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? RoomName { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Note { get; set; }        
    }
}
