using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Models;
using TECH.Models.Search;
using TECH.Service;

namespace TECH.Controllers
{
    public class DepartmentController: Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new DepartmentModelView();
            if (id > 0)
            {
                model = _departmentService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetAllDepartment()
        {
            var model = _departmentService.GetAll();
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
        public JsonResult Add(DepartmentModelView departmentModelView)
        {
            if (_departmentService.IsExist(departmentModelView.RoomName))
            {
                return Json(new
                {
                    success = false
                });
            }
            _departmentService.Add(departmentModelView);
            _departmentService.Save();
            return Json(new
            {
                success = true
            });

        }       

        [HttpPost]
        public JsonResult Update(DepartmentModelView departmentModelView)
        {           
            var result = _departmentService.Update(departmentModelView);
            _departmentService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _departmentService.Deleted(id);
            _departmentService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public JsonResult GetAllPaging(SearchModelView search)
        {
            var data = _departmentService.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
