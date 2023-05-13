using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Models
{
    public class HomeModelView
    {
        public int DepartmentCount { get; set; }
        public int PositionCount { get; set; }
        public int ProjectCount { get; set; }
        public int EmployeeCount { get; set; }
        public Dictionary<string,List<PositionEmployeeView>> LstData { get; set; }

    }
    public class PositionEmployeeView
    {
        public string? PositionName { get; set; }
        public int EmployeeCount { get; set; }

    }

}
