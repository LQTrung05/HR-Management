using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Models;
using TECH.Models.Search;
using TECH.Service;

namespace TECH.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new PositionModelView();
            if (id > 0)
            {
                model = _positionService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetPositionChucVu()
        {
            var model = _positionService.GetAll();
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
        public JsonResult Add(PositionModelView PositionModelView)
        {
            if (_positionService.IsExist(PositionModelView.PositionName))
            {
                return Json(new
                {
                    success = false
                });
            }
            _positionService.Add(PositionModelView);
            _positionService.Save();
            return Json(new
            {
                success = true
            });

        }       

        [HttpPost]
        public JsonResult Update(PositionModelView PositionModelView)
        {           
            var result = _positionService.Update(PositionModelView);
            _positionService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _positionService.Deleted(id);
            _positionService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public JsonResult GetAllPaging(SearchModelView search)
        {
            var data = _positionService.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
