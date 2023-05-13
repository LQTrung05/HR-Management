using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TECH.Models;
using TECH.Models.Search;
using System.Net.Mail;
using TECH.Service;

namespace TECH.Controllers
{
    public class UsersController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;
        public IHttpContextAccessor _httpContextAccessor;
        public UsersController(IEmployeeService employeeService,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
        }  
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
           
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AppLogin(string userName, string passWord)
        {
            var result = _accountService.AppUserLogin(userName, passWord);

            if (result != null && !string.IsNullOrEmpty(result.EmployeeCode))
            {
                var userLogin = _employeeService.GetByid(result.EmployeeCode);
                if (userLogin != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(userLogin));
                    return Json(new
                    {
                        success = true,
                        data = result
                    });
                }
              
            }
            return Json(new
            {
                success = false
            });
        }


        //[HttpPost]
        //public JsonResult AddRegister(UserModelView UserModelView)
        //{
        //    bool isMailExist = false;
        //    bool isPhoneExist = false;
        //    if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.email))
        //    {
        //        var isMail = _appUserService.IsMailExist(UserModelView.email);
        //        if (isMail)
        //        {
        //            isMailExist = true;
        //        }
        //    }

        //    if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.phone_number))
        //    {
        //        var isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number);
        //        if (isPhone)
        //        {
        //            isPhoneExist = true;
        //        }
        //    }

        //    if (!isMailExist && !isPhoneExist)
        //    {
        //        var result = _appUserService.Add(UserModelView);
        //        _appUserService.Save();
        //        if (result > 0)
        //        {
        //            var _user = _appUserService.GetByid(result);
        //            _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(_user));
        //        }
        //        return Json(new
        //        {
        //            success = result
        //        });
        //    }
        //    return Json(new
        //    {
        //        success = false,
        //        isMailExist = isMailExist,
        //        isPhoneExist = isPhoneExist
        //    });

        //}
        //public IActionResult Profile()
        //{
        //    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
        //    var user = new UserModelView();
        //    if (userString != null)
        //    {
        //        user = JsonConvert.DeserializeObject<UserModelView>(userString);
        //        if (!string.IsNullOrEmpty(user.address) && user.address != "null")
        //        {
        //            user.address = user.address;
        //        }
        //        else
        //        {
        //            user.address = "";
        //        }
        //        return View(user);
        //    }
        //    return Redirect("/home");

        //}

        //public JsonResult AppLogin(UserModelView UserModelView)
        //{
        //    var result = _appUserService.Add(UserModelView);
        //    _appUserService.Save();
        //    return Json(new
        //    {
        //        success = result
        //    });

        //}

        public IActionResult LogOut()
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            if (userString != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove("UserInfor");
            }

            return Redirect("/");

        }
        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new EmployeeModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<EmployeeModelView>(userString);
                if (user != null)
                {
                    var dataUser = _employeeService.GetByid(user.EmployeeCode);
                    model = dataUser;
                }

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassWord()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new EmployeeModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<EmployeeModelView>(userString);
                if (user != null)
                {
                    var dataUser = _employeeService.GetByid(user.EmployeeCode);
                    model = dataUser;
                }
                return View(model);
            }
            return Redirect("/Home");

        }
        [HttpPost]
        public JsonResult ChangeServerPassWord(string userId, string current_password, string new_password)
        {
            var model = _employeeService.ChangePassWord(userId, current_password, new_password);
            _employeeService.Save();
            return Json(new
            {
                success = model
            });
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(EmployeeModelView EmployeeModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new EmployeeModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<EmployeeModelView>(userString);
                if (user != null)
                {
                    var dataUser = _employeeService.GetByid(user.EmployeeCode);
                    if (dataUser != null)
                    {

                        //if (dataUser.Email.ToLower().Trim() != EmployeeModelView.Email.ToLower().Trim())
                        //{
                        //    bool isEmail = _employeeService.IsMailExist(EmployeeModelView.Email.ToLower().Trim());
                        //    if (isEmail)
                        //    {
                        //        return Json(new
                        //        {
                        //            success = false,
                        //            isExistEmail = true,
                        //            isExistPhone = false,
                        //        });
                        //    }
                        //}

                        //if (dataUser.SoDienThoai.ToLower().Trim() != NhanVienModelView.SoDienThoai.ToLower().Trim())
                        //{
                        //    bool isPhone = _nhanVienService.IsPhoneExist(NhanVienModelView.SoDienThoai.ToLower().Trim());
                        //    if (isPhone)
                        //    {
                        //        return Json(new
                        //        {
                        //            success = false,
                        //            isExistEmail = false,
                        //            isExistPhone = true,
                        //        });
                        //    }
                        //}

                        EmployeeModelView.EmployeeCode = dataUser.EmployeeCode;
                        status = _employeeService.UpdateDetailView(EmployeeModelView);
                        _employeeService.Save();
                        _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(EmployeeModelView));
                        //dataUser = _nhanVienService.GetByid(user.id);
                        //_httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(dataUser));
                        return Json(new
                        {
                            success = status,
                            isExistEmail = false,
                            isExistPhone = false,
                        });
                    }
                }
            }


            return Json(new
            {
                success = status
            });
        }




        //public IActionResult ChangePass()
        //{
        //    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
        //    var model = new UserModelView();
        //    if (userString != null)
        //    {
        //        var user = JsonConvert.DeserializeObject<UserModelView>(userString);
        //        if (user != null)
        //        {
        //            var dataUser = _appUserService.GetByid(user.id);
        //            model = dataUser;
        //        }
        //        return View(model);
        //    }
        //    return Redirect("/home");
        //}
    }
}
