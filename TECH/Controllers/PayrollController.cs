using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Models;
using TECH.Models.Search;
using TECH.Service;

namespace TECH.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IPayrollService _payrollService;
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        private readonly IBonusPunishService _bonusPunishService;
        private readonly IDepartmentService _departmentService;
        public PayrollController(IPayrollService payrollService,
            IEmployeeService employeeService,
            IBonusPunishService bonusPunishService,
            IDepartmentService departmentService,
        IPositionService positionService)
        {
            _payrollService = payrollService;
            _employeeService = employeeService;
            _positionService = positionService;
            _departmentService = departmentService;
            _bonusPunishService = bonusPunishService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new PayrollModelView();
            if (id > 0)
            {
                model = _payrollService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetThuongPhatBonus(int id)
        {
            var model = new List<BonusPunishModelView>();
            if (id > 0)
            {
                model = _bonusPunishService.GetByPayRollId(id);
            }
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
        
        //[HttpPost]
        //public JsonResult Add(PayrollModelView PayrollModelView)
        //{
        //    if (PayrollModelView != null && !string.IsNullOrEmpty(PayrollModelView.EmployeeCode) )
        //    {
        //        decimal totalLuong = 0;
        //        var employee = _employeeService.GetByid(PayrollModelView.EmployeeCode);
        //        if (employee != null && employee.PositionCode.HasValue && employee.PositionCode.Value > 0)
        //        {
        //            var position = _positionService.GetByid(employee.PositionCode.Value);
        //            if (position != null && position.Coefficient.HasValue && position.Coefficient.Value > 0)
        //            {
        //                totalLuong = (PayrollModelView.Basicsalary.HasValue && PayrollModelView.Basicsalary.Value > 0 ? PayrollModelView.Basicsalary.Value : 0) * Convert.ToDecimal(position.Coefficient.Value);
        //            }
        //        }
        //        if (PayrollModelView.SalaryKPI.HasValue && PayrollModelView.SalaryKPI.Value > 0)
        //        {
        //            totalLuong += PayrollModelView.SalaryKPI.Value;
        //        }
        //        PayrollModelView.TotalSalary = totalLuong;
        //    }
        //    _payrollService.Add(PayrollModelView);
        //    _payrollService.Save();
        //    return Json(new
        //    {
        //        success = true
        //    });

        //}       

        //[HttpPost]
        //public JsonResult Update(PayrollModelView PayrollModelView)
        //{
        //    if (PayrollModelView != null && !string.IsNullOrEmpty(PayrollModelView.EmployeeCode))
        //    {
        //        decimal totalLuong = 0;
        //        var employee = _employeeService.GetByid(PayrollModelView.EmployeeCode);
        //        if (employee != null && employee.PositionCode.HasValue && employee.PositionCode.Value > 0)
        //        {
        //            var position = _positionService.GetByid(employee.PositionCode.Value);
        //            if (position != null && position.Coefficient.HasValue && position.Coefficient.Value > 0)
        //            {
        //                totalLuong = (PayrollModelView.Basicsalary.HasValue && PayrollModelView.Basicsalary.Value > 0 ? PayrollModelView.Basicsalary.Value : 0) * Convert.ToDecimal(position.Coefficient.Value);
        //            }
        //        }
        //        if (PayrollModelView.SalaryKPI.HasValue && PayrollModelView.SalaryKPI.Value > 0)
        //        {
        //            totalLuong += PayrollModelView.SalaryKPI.Value;
        //        }
        //        PayrollModelView.TotalSalary = totalLuong;
        //    }

        //    var result = _payrollService.Update(PayrollModelView);
        //    _payrollService.Save();
        //    return Json(new
        //    {
        //        success = result
        //    });
        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _payrollService.Deleted(id);
            _payrollService.Save();
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
                    var totalLuong = item.Basicsalary.HasValue && item.Basicsalary.Value > 0 ? item.Basicsalary.Value:0; 
                    if (item.PositionCode.HasValue && item.PositionCode.Value > 0)
                    {
                        var position = _positionService.GetByid(item.PositionCode.Value);
                        if (position != null)
                        {
                            item.PositionCodeStr = position.PositionName;
                            if (position.Coefficient.HasValue && position.Coefficient.Value > 0)
                            {
                                totalLuong = totalLuong * Convert.ToDecimal(position.Coefficient.Value);
                                item.Coefficient = position.Coefficient.Value;
                            }
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
                    // tính thưởng phạt
                    var bonus = _bonusPunishService.GetByPayRollByEmploye(item.EmployeeCode);
                    if (bonus != null && bonus.Count > 0)
                    {
                        var thuong = bonus.Where(p => p.RewardType == 1).Sum(p => p.PenaltyBonus.Value);
                        item.TotalBonusStr = thuong > 0 ? thuong.ToString("#,###") : "";
                        var phat = bonus.Where(p => p.RewardType == 2).Sum(p => p.PenaltyBonus.Value);
                        item.TotalPunishStr = phat > 0 ? phat.ToString("#,###") : "";
                        totalLuong = totalLuong + thuong - phat;
                    }
                    item.TotalPenaltyBonusStr = totalLuong > 0 ? totalLuong.ToString("#,###") : "";
                }
            }
            return Json(new { data = data });

            //var data = _payrollService.GetAllPaging(search);
            //if (data != null && data.Results != null && data.Results.Count > 0)
            //{
            //    foreach (var item in data.Results)
            //    {
            //        decimal totalLuong = 0;
            //        if (!string.IsNullOrEmpty(item.EmployeeCode))
            //        {
            //            item.Employee = _employeeService.GetByid(item.EmployeeCode);
            //            if (item.Employee != null && item.Employee.PositionCode.HasValue && item.Employee.PositionCode.Value > 0)
            //            {
            //                var postion = _positionService.GetByid(item.Employee.PositionCode.Value);
            //                if (postion != null && postion.Coefficient.HasValue && postion.Coefficient.Value > 0)
            //                {
            //                    item.Coefficient = postion.Coefficient.Value.ToString();
            //                    totalLuong = item.Basicsalary.HasValue && item.Basicsalary.Value > 0 ? item.Basicsalary.Value * Convert.ToDecimal(postion.Coefficient.Value) : 0;
            //                }
            //            }
            //        }
            //        if (item.SalaryKPI.HasValue && item.SalaryKPI.Value > 0)
            //        {
            //            totalLuong += item.SalaryKPI.Value;
            //        }
            //        if (item.Id > 0)
            //        {
            //            var bonus = _bonusPunishService.GetByPayRollId(item.Id);
            //            if (bonus != null && bonus.Count > 0)
            //            {
            //                item.BonusPunish = bonus;
            //                var thuong = bonus.Where(p => p.RewardType == 1).Sum(p => p.PenaltyBonus.Value);
            //                item.TotalBonusStr = thuong > 0 ? thuong.ToString("#,###") : "";
            //                var phat = bonus.Where(p => p.RewardType == 2).Sum(p => p.PenaltyBonus.Value);
            //                item.TotalPunishStr = phat > 0 ? phat.ToString("#,###") : "";
            //                totalLuong = totalLuong + thuong - phat;
            //            }
            //        }
            //        item.TotalPenaltyBonusStr = totalLuong > 0 ? totalLuong.ToString("#,###"):"";
            //    }
            //}
            return Json(new { data = data });
        }
    }
}
