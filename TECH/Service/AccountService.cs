
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Models;
using TECH.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IAccountService
    {
        AccountModelView GetByid(string employeeCode);
        void Add(AccountModelView view);
        bool Update(AccountModelView view);
        bool Deleted(string employeeCode);
        void Save();
        bool IsExist(string name);
        AccountModelView AppUserLogin(string userName, string passWord);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        public AccountService(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }
        public AccountModelView AppUserLogin(string userName, string passWord)
        {
            var data = _accountRepository.FindAll().Where(u => u.UserName == userName && u.PassWord == passWord).FirstOrDefault();
            if (data != null)
            {
                var model = GetByid(data.EmployeeCode);
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }
        public bool IsExist(string name)
        {
            var data = _accountRepository.FindAll().Any(p => p.UserName == name);
            return data;
        }
        public AccountModelView GetByid(string employeeCode)
        {
            var data = _accountRepository.FindAll(p => p.EmployeeCode == employeeCode).FirstOrDefault();
            if (data != null)
            {
                var model = new AccountModelView()
                {
                    EmployeeCode = data.EmployeeCode,
                    UserName = !string.IsNullOrEmpty(data.UserName) ?data.UserName : "",
                    PassWord = !string.IsNullOrEmpty(data.PassWord) ? data.PassWord : ""
                };
                return model;
            }
            return null;
        }
        public void Add(AccountModelView view)
        {
            try
            {
                if (view != null)
                {
                    var project = new Account
                    {
                        EmployeeCode = !string.IsNullOrEmpty(view.EmployeeCode) ? view.EmployeeCode : "",
                        UserName = !string.IsNullOrEmpty(view.UserName) ? view.UserName : "",
                        PassWord = !string.IsNullOrEmpty(view.PassWord) ? view.PassWord : ""
                    };
                    _accountRepository.Add(project);
                }
            }
            catch (Exception ex)
            {
            }

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(AccountModelView view)
        {
            try
            {
                var dataServer = _accountRepository.FindAll(p=>p.EmployeeCode == view.EmployeeCode).FirstOrDefault();
                if (dataServer != null)
                {
                    //if (dataServer.UserName.ToLower().Trim() != view.UserName.ToLower().Trim())
                    //{
                    //    if (IsExist(view.UserName))
                    //    {
                    //        return false;
                    //    }
                    //}
                    dataServer.UserName = !string.IsNullOrEmpty(view.UserName) ? view.UserName : "";
                    dataServer.PassWord = !string.IsNullOrEmpty(view.PassWord) ? view.PassWord : "";
                    _accountRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }      
        public bool Deleted(string employeeCode)
        {
            try
            {
                var dataServer = _accountRepository.FindAll(p=>p.EmployeeCode == employeeCode).FirstOrDefault();
                if (dataServer != null)
                {
                    _accountRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        
    }
}
