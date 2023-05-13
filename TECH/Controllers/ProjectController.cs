using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Models;
using TECH.Models.Search;
using TECH.Service;

namespace TECH.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeProjectService _employeeProjectService;
        private readonly IEmployeeService _employeeService;
        public ProjectController(IProjectService projectService,
            IEmployeeService employeeService,
        IEmployeeProjectService employeeProjectService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
            _employeeProjectService = employeeProjectService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ProjectModelView();
            if (id > 0)
            {
                model = _projectService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetAllProject()
        {
            var model = _projectService.GetAll();
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult Add(ProjectModelView ProjectModelView)
        {
            if (_projectService.IsExist(ProjectModelView.ProjectName))
            {
                return Json(new
                {
                    success = false
                });
            }
            _projectService.Add(ProjectModelView);
            _projectService.Save();
            return Json(new
            {
                success = true
            });

        }       

        [HttpPost]
        public JsonResult Update(ProjectModelView ProjectModelView)
        {           
            var result = _projectService.Update(ProjectModelView);
            _projectService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _projectService.Deleted(id);
            _projectService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public JsonResult GetAllPaging(SearchModelView search)
        {
            var data = _projectService.GetAllPaging(search);
            return Json(new { data = data });
        }
        [HttpPost]
        public JsonResult AddThanhVienProject(List<EmployeeProjectModelView> ThanhVienProjectModelViews, int maProject)
        {
            if (maProject > 0)
            {
                _employeeProjectService.DeletedByMaPhong(maProject);
                if (ThanhVienProjectModelViews != null && ThanhVienProjectModelViews.Count > 0)
                {
                    foreach (var item in ThanhVienProjectModelViews)
                    {
                        if (!string.IsNullOrEmpty(item.EmployeeCode))
                        {
                            _employeeProjectService.Add(item);
                        }
                    }
                }
                _employeeProjectService.Save();
                return Json(new
                {
                    success = true
                });
            }
            return Json(new
            {
                success = false
            });
        }
        [HttpGet]
        public JsonResult GetThanhVienProjectByProjectId(int id)
        {
            var model = _employeeProjectService.GetThanhVienByMaProject(id);
            if (model != null && model.Count > 0)
            {
                foreach (var item in model)
                {
                    if (!string.IsNullOrEmpty(item.EmployeeCode))
                    {
                        var employee = _employeeService.GetByid(item.EmployeeCode);
                        item.Employee = employee;
                    }
                }
            }
            return Json(new
            {
                Data = model
            });
        }
    }
}
