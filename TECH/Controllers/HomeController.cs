using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;
        private readonly IProjectService _projectService;
        public HomeController(ILogger<HomeController> logger,
            IEmployeeService employeeService,
            IPositionService positionService,
            IProjectService projectService,
            IDepartmentService departmentService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _positionService = positionService;
            _projectService = projectService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var home = new HomeModelView();
            home.EmployeeCount = _employeeService.GetCount();
            home.PositionCount = _positionService.GetCount();
            home.ProjectCount = _projectService.GetCount();
            home.DepartmentCount = _departmentService.GetCount();
            var DepartmentData = _departmentService.GetAll();
            var data = _employeeService.GetAll();
            if (data != null && data.Count > 0)
            {
                Dictionary<string, List<PositionEmployeeView>> LstData = new Dictionary<string, List<PositionEmployeeView>>();
                
                foreach (var item in data)
                {
                    if (item.PositionCode.HasValue && item.PositionCode.Value > 0)
                    {
                        var position = _positionService.GetByid(item.PositionCode.Value);
                        if (position != null)
                        {
                            item.PositionCodeStr = position.PositionName;
                        }
                    }
                    if (item.RoomCode.HasValue && item.RoomCode.Value > 0)
                    {
                        var room = _departmentService.GetByid(item.RoomCode.Value);
                        if (room != null)
                        {
                            item.RoomCodeStr = room.RoomName;
                        }
                    }
                }
                var dataRoom = data.GroupBy(p=>p.RoomCodeStr);
                if (dataRoom != null)
                {
                    var lstPostion = new List<EmployeeModelView>();
                  
                    foreach (var item in dataRoom)
                    {
                        var lstPostionEmployee = new List<PositionEmployeeView>();
                        lstPostion = item.ToList();
                        var lstPostionGroup = lstPostion.GroupBy(p=>p.PositionCodeStr).ToDictionary(p=>p.Key,p=>p.ToList());

                        if (lstPostionGroup != null && lstPostionGroup.Count() > 0)
                        {
                          
                            foreach (var itemEmployee in lstPostionGroup)
                            {
                                var postionEmployee = new PositionEmployeeView();
                                postionEmployee.PositionName = itemEmployee.Key;
                                postionEmployee.EmployeeCount = itemEmployee.Value.Count();
                                lstPostionEmployee.Add(postionEmployee);
                            }                            
                        }
                        LstData.Add(item.Key, lstPostionEmployee);
                    }
                    //if (LstData != null && LstData.Count > 0)
                    //{
                        
                    //}
                    home.LstData = LstData;
                }
            }
            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}