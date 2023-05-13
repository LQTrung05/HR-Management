
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TECH.Models;
using TECH.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Utilities;
using System.Data;
using Newtonsoft.Json;

namespace TECH.Service
{
    public interface IPayrollService
    {
        PagedResult<PayrollModelView> GetAllPaging(SearchModelView PayrollModelViewSearch);
        PayrollModelView GetByid(int id);
        //PayrollModelView Add(PayrollModelView view);
        //bool Update(PayrollModelView view);
        void Save();
        //bool IsExist(string name);
        bool Deleted(int id);
    }

    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private IUnitOfWork _unitOfWork;
        public PayrollService(IPayrollRepository payrollRepository,
        IUnitOfWork unitOfWork)
        {
            _payrollRepository = payrollRepository;
            _unitOfWork = unitOfWork;
        }
        public PayrollModelView GetByid(int id)
        {
            var data = _payrollRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new PayrollModelView()
                {
                    Id = data.Id,
                    EmployeeCode = data.EmployeeCode,
                    SalaryKPI = data.SalaryKPI,
                    Basicsalary=data.Basicsalary,
                    BasicsalaryStr = data.Basicsalary.HasValue && data.Basicsalary.Value > 0 ? data.Basicsalary.Value.ToString("#,###") : "",
                    SalaryKPIStr = data.SalaryKPI.HasValue && data.SalaryKPI.Value > 0 ? data.SalaryKPI.Value.ToString("#,###") : "",
                    Note = data.Note                   
                };
                return model;
            }
            return null;
        }
        //public PayrollModelView Add(PayrollModelView view)
        //{
        //    try
        //    {
        //        if (view != null)
        //        {
        //            var payroll = new Payroll
        //            {
        //                EmployeeCode = view.EmployeeCode,
        //                Basicsalary= view.Basicsalary,
        //                SalaryKPI = view.SalaryKPI,
        //                TotalSalary = view.TotalSalary,
        //                Note = view.Note
        //            };
        //            _payrollRepository.Add(payroll);
        //            Save();
        //            return view;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return null;

        //}
        public void Save()
        {
            _unitOfWork.Commit();
        }
        //public bool Update(PayrollModelView view)
        //{
        //    try
        //    {
        //        var dataServer = _payrollRepository.FindAll(p => p.Id == view.Id).FirstOrDefault();
        //        if (dataServer != null)
        //        {
        //            dataServer.EmployeeCode = view.EmployeeCode;
        //            dataServer.SalaryKPI = view.SalaryKPI;
        //            dataServer.Basicsalary = view.Basicsalary;
        //            dataServer.Note = view.Note;
        //            dataServer.TotalSalary = view.TotalSalary;
        //            _payrollRepository.Update(dataServer);
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return false;
        //}
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _payrollRepository.FindAll(p => p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {                  
                    _payrollRepository.Remove(dataServer);
                    Save();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return false;
        }       
        public PagedResult<PayrollModelView> GetAllPaging(SearchModelView PayrollModelViewSearch)
        {
            try
            {
                var query = _payrollRepository.FindAll();
                if (!string.IsNullOrEmpty(PayrollModelViewSearch.selectedNhanVien))
                {
                    query = query.Where(c => c.EmployeeCode.ToLower().Trim().Contains(PayrollModelViewSearch.selectedNhanVien.ToLower().Trim()));
                }
                int totalRow = query.Count();
                query = query.Skip((PayrollModelViewSearch.PageIndex - 1) * PayrollModelViewSearch.PageSize).Take(PayrollModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.EmployeeCode).Select(c => new PayrollModelView()
                {
                    Id = c.Id,
                    Basicsalary = c.Basicsalary,
                    BasicsalaryStr = c.Basicsalary.HasValue && c.Basicsalary.Value > 0 ? c.Basicsalary.Value.ToString("#,###") :"",
                    EmployeeCode = c.EmployeeCode,                  
                    SalaryKPI = c.SalaryKPI,
                    SalaryKPIStr = c.SalaryKPI.HasValue && c.SalaryKPI.Value > 0 ? c.SalaryKPI.Value.ToString("#,###"):"",
                    Note = !string.IsNullOrEmpty(c.Note)? c.Note:"",
                    TotalSalaryStr = c.TotalSalary.HasValue && c.TotalSalary.Value > 0 ? c.TotalSalary.Value.ToString("#,###") : "",
                    TotalSalary = c.TotalSalary
                }).ToList();

                var pagingData = new PagedResult<PayrollModelView>
                {
                    Results = data,
                    CurrentPage = PayrollModelViewSearch.PageIndex,
                    PageSize = PayrollModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
