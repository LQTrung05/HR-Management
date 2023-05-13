using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Models;
using TECH.Models.Search;
using TECH.Service;

namespace TECH.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;
        private readonly IBonusPunishService _bonusPunishService;
        private readonly IAccountService _accountService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public EmployeeController(IEmployeeService employeeService,
            IAccountService accountService,
            IDepartmentService departmentService,
            IPositionService positionService,
            IBonusPunishService bonusPunishService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _departmentService= departmentService;
            _positionService= positionService;
            _hostingEnvironment = hostingEnvironment;
            _bonusPunishService = bonusPunishService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(string id)
        {
            var model = new EmployeeModelView();
            if (!string.IsNullOrEmpty(id))
            {
                model = _employeeService.GetByid(id);
                if (model != null && !string.IsNullOrEmpty(model.EmployeeCode))
                {
                    var acount = _accountService.GetByid(model.EmployeeCode);
                    if (acount != null && 
                        !string.IsNullOrEmpty(acount.UserName) && 
                        !string.IsNullOrEmpty(acount.PassWord))
                    {
                        model.UserName = acount.UserName;
                        model.Password = acount.PassWord;
                    }
                }
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var model = _employeeService.GetAll();           
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
        public JsonResult Add(EmployeeModelView EmployeeModelView)
        {
            if (_employeeService.IsExist(EmployeeModelView.Email) || _employeeService.IsExist(EmployeeModelView.SDT))
            {
                return Json(new
                {
                    success = false
                });
            }

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");           
            if (userString != null)
            {
                var userLogin = JsonConvert.DeserializeObject<EmployeeModelView>(userString);              
                if (userLogin != null && !string.IsNullOrEmpty(userLogin.FullName))
                {
                    EmployeeModelView.CreatedBy = userLogin.FullName;
                }
            }

            var data = _employeeService.Add(EmployeeModelView);
            if (data != null)
            {
                var acountView = new AccountModelView()
                {
                    EmployeeCode = data.EmployeeCode,
                    UserName = data.UserName,
                    PassWord = data.Password
                };
                _accountService.Add(acountView);
                _accountService.Save();
            }
            
            return Json(new
            {
                success = true,
                id= data.EmployeeCode
            });

        }       

        [HttpPost]
        public JsonResult Update(EmployeeModelView EmployeeModelView)
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            if (userString != null)
            {
                var userLogin = JsonConvert.DeserializeObject<EmployeeModelView>(userString);
                if (userLogin != null && !string.IsNullOrEmpty(userLogin.FullName))
                {
                    EmployeeModelView.UpdatedBy = userLogin.FullName;
                }
            }
            var result = _employeeService.Update(EmployeeModelView);
            _employeeService.Save();
            // update account
            if (!string.IsNullOrEmpty(EmployeeModelView.EmployeeCode))
            {
                var account = _accountService.GetByid(EmployeeModelView.EmployeeCode);
                if (account != null)
                {
                    account.UserName = EmployeeModelView.UserName;
                    account.PassWord = EmployeeModelView.Password;
                    _accountService.Update(account);
                    _accountService.Save();
                }
            }
            
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var result = _employeeService.Deleted(id);
            _employeeService.Save();
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            if (!string.IsNullOrEmpty(userString))
            {
                var user = JsonConvert.DeserializeObject<EmployeeModelView>(userString);
                if (user != null && id == user.EmployeeCode)
                {
                    _httpContextAccessor.HttpContext.Session.Remove("UserInfor");
                    //return Json(new
                    //{
                    //    success = false,                        
                    //});
                }
            }
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public JsonResult GetAllPaging(EmployeeModelViewSearch search)
        {
            var data = _employeeService.GetAllPaging(search);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
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
            }
            return Json(new { data = data });
        }
        [HttpPost]
        public IActionResult UploadImageAvartar()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\avartar\";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                foreach (var itemFile in files)
                {
                    string fileNameFormat = Regex.Replace(itemFile.FileName.ToLower(), @"\s+", "");
                    if (!string.IsNullOrEmpty(itemFile.Name))
                    {
                        var employeeServer = _employeeService.GetByid(itemFile.Name);
                        if (employeeServer != null)
                        {
                            employeeServer.Avatar = fileNameFormat;
                            _employeeService.UpdateAvatar(employeeServer.EmployeeCode, employeeServer.Avatar);
                        }
                    }                 
                    string filePath = Path.Combine(folder, fileNameFormat);
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            itemFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            return Json(new
            {
                success = true
            });

        }

        [HttpPost]
        public JsonResult AddTThuongPhat(List<BonusPunishModelView> BonusPunishModelViews, string employeeCode)
        {
            if (!string.IsNullOrEmpty(employeeCode))
            {
                _bonusPunishService.DeletedByEmpployeeCode(employeeCode);
                if (BonusPunishModelViews != null && BonusPunishModelViews.Count > 0)
                {
                    foreach (var item in BonusPunishModelViews)
                    {
                        if (item.RewardType.HasValue && item.RewardType.Value > 0)
                        {
                            _bonusPunishService.Add(item);
                        }
                    }
                }
            }
            _bonusPunishService.Save();
            return Json(new
            {
                success = true
            });
        }

        //[HttpGet]
        //public JsonResult GetThuongPhatByRollId(int id)
        //{
        //    var model = _bonusPunishService.GetByPayRollId(id);
        //    if (model != null && model.Count > 0)
        //    {
        //        //foreach (var item in model)
        //        //{
        //        //    if (!string.IsNullOrEmpty(item.EmployeeCode))
        //        //    {
        //        //        var employee = _employeeService.GetByid(item.EmployeeCode);
        //        //        item.Employee = employee;
        //        //    }
        //        //}
        //    }
        //    return Json(new
        //    {
        //        Data = model
        //    });
        //}
        [HttpGet]
        public JsonResult GetThuongPhatEmployee(string  employeeCode)
        {
            var model = _bonusPunishService.GetByForEmployee(employeeCode);           
            return Json(new
            {
                Data = model
            });
        }
        [HttpPost]
        public JsonResult DeleteThuongPhat(string employeeCode)
        {
             _bonusPunishService.DeletedByEmpployeeCode(employeeCode);
            return Json(new
            {
                success = true
            });
        }

        public IActionResult Luong()
        {
            return View();
        }
        
    }
}
