
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
    public interface IEmployeeService
    {
        PagedResult<EmployeeModelView> GetAllPaging(EmployeeModelViewSearch EmployeeModelViewSearch);
        EmployeeModelView GetByid(string id);
        List<EmployeeModelView> GetAll();
        EmployeeModelView Add(EmployeeModelView view);
        bool Update(EmployeeModelView view);
        void UpdateAvatar(string employeeCode, string avatar);
        void Save();
        bool IsExist(string name);
        bool Deleted(string id);
        bool UpdateDetailView(EmployeeModelView view);
        bool ChangePassWord(string userId, string current_password, string new_password, bool isCofirm = false);
        int GetCount();
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        public EmployeeService(IEmployeeRepository employeeRepository,
             IAccountRepository accountRepository,
            IPositionRepository positionRepository,
        IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }
        public int GetCount()
        {
            int count = 0;
            count = _employeeRepository.FindAll(p=>p.IsDeleted.HasValue && p.IsDeleted.Value != true).Count();
            return count;
        }
        public bool UpdateDetailView(EmployeeModelView view)
        {
            try
            {
                var dataServer = _employeeRepository.FindAll(p=>p.EmployeeCode == view.EmployeeCode).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.Address = view.Address;
                    dataServer.FullName = view.FullName;
                    dataServer.Email = view.Email;
                    dataServer.SDT = view.SDT;
                    _employeeRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public bool ChangePassWord(string userId, string current_password, string new_password, bool isCofirm = false)
        {
            var modelUser = _employeeRepository.FindAll().Where(u => u.EmployeeCode == userId).FirstOrDefault();
            bool status = false;
            if (modelUser != null)
            {
                var checkacount = _accountRepository.FindAll(p => p.EmployeeCode == userId && p.PassWord == current_password).FirstOrDefault();
                if (checkacount != null)
                {
                    checkacount.PassWord = new_password;
                    _accountRepository.Update(checkacount);
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }
        public bool IsExist(string name)
        {
            var data = _employeeRepository.FindAll().Any(p => p.Email == name || p.SDT == name);
            return data;
        }
        public EmployeeModelView GetByid(string id)
        {
            var data = _employeeRepository.FindAll(p => p.EmployeeCode == id && p.IsDeleted.HasValue && p.IsDeleted.Value !=true).FirstOrDefault();
            if (data != null)
            {
                var model = new EmployeeModelView()
                {
                    EmployeeCode = data.EmployeeCode,
                    PositionCode = data.PositionCode,
                    RoomCode = data.RoomCode,
                    Avatar = data.Avatar,
                    FullName = data.FullName,
                    SDT = data.SDT,
                    Email = data.Email,
                    CMND = data.CMND,
                    LevelAcademic = data.LevelAcademic,
                    Gender = data.Gender,
                    GenderStr = data.Gender.HasValue ? Common.GetGender(data.Gender.Value) : "",
                    YearOfBirth = data.YearOfBirth,
                    YearOfBirthStr = data.YearOfBirth.HasValue ? data.YearOfBirth.Value.ToString("yyyy-MM-dd") : "",
                    Address = data.Address,
                    IsDeleted = data.IsDeleted,
                    CreatedBy = data.CreatedBy,
                    CreatedDated = data.CreatedDated,
                    CreatedDatedStr = data.CreatedDated.HasValue ? data.CreatedDated.Value.ToString("yyyy-MM-dd") : "",
                    UpdatedBy = data.UpdatedBy,                    
                    UpdatedDated = data.UpdatedDated,
                    Basicsalary=data.Basicsalary,
                    UpdatedDatedStr = data.UpdatedDated.HasValue ? data.UpdatedDated.Value.ToString("yyyy-MM-dd") : "",
                };
                return model;
            }
            return null;
        }
        public List<EmployeeModelView> GetAll()
        {
            var data = _employeeRepository.FindAll(p=>p.IsDeleted.HasValue && p.IsDeleted.Value != true).Select(c => new EmployeeModelView()
            {
                EmployeeCode = c.EmployeeCode,
                PositionCode = c.PositionCode,
                RoomCode = c.RoomCode,
                Avatar = c.Avatar,
                FullName = c.FullName,
                SDT = c.SDT,
                Email = c.Email,
                CMND = c.CMND,
                LevelAcademic = c.LevelAcademic,
                LevelAcademicStr = c.LevelAcademic.HasValue ? Common.GetGender(c.LevelAcademic.Value) : "",
                Gender = c.Gender,
                GenderStr = c.Gender.HasValue ? Common.GetGender(c.Gender.Value) : "",
                YearOfBirth = c.YearOfBirth,
                YearOfBirthStr = c.YearOfBirth.HasValue ? c.YearOfBirth.Value.ToString("yyyy-MM-dd") : "",
                Address = c.Address,
                IsDeletedStr = c.IsDeleted.HasValue ? Common.GetEmployStatus(c.IsDeleted.Value) : "",
                IsDeleted = c.IsDeleted,
                CreatedBy = c.CreatedBy,
                CreatedDated = c.CreatedDated,
                CreatedDatedStr = c.CreatedDated.HasValue ? c.CreatedDated.Value.ToString("yyyy-MM-dd") : "",
                UpdatedBy = c.UpdatedBy,
                UpdatedDated = c.UpdatedDated,
                Basicsalary=c.Basicsalary,
                UpdatedDatedStr = c.UpdatedDated.HasValue ? c.UpdatedDated.Value.ToString("yyyy-MM-dd") : "",
            }).ToList();
            return data;
        }
        public EmployeeModelView Add(EmployeeModelView view)
        {
            try
            {
                if (view != null)
                {
                    var employeeAllServer = _employeeRepository.FindAll();
                        
                        string maTuDong = "NV";
                        if (employeeAllServer != null && employeeAllServer.Count() > 0)
                        {
                            string employeeCodeLast = employeeAllServer.OrderByDescending(p=>p.EmployeeCode).FirstOrDefault()?.EmployeeCode;
                            if (!string.IsNullOrEmpty(employeeCodeLast) && employeeCodeLast.IndexOf(maTuDong) >= 0)
                            {
                                string number = employeeCodeLast.Substring(maTuDong.Length);
                                int k = Convert.ToInt32(number);
                                k = k + 1;
                                if (k < 10)
                                {
                                    maTuDong = maTuDong + "00";
                                }
                                else if (k < 100)
                                {
                                    maTuDong = maTuDong + "0";
                                }
                                maTuDong = maTuDong + k.ToString();
                            }
                            else
                            {
                                maTuDong = maTuDong + "001";
                            }

                        }
                        else
                        {
                            maTuDong = maTuDong + "001";
                        }
                    var position = new Employee
                    {
                        EmployeeCode = maTuDong,
                        PositionCode = view.PositionCode,
                        RoomCode = view.RoomCode,
                        Avatar = view.Avatar,
                        FullName = view.FullName,
                        SDT = view.SDT,
                        Email = view.Email,
                        CMND = view.CMND,
                        LevelAcademic = view.LevelAcademic,
                        Gender = view.Gender,
                        YearOfBirth = view.YearOfBirth,
                        Address = view.Address,
                        CreatedBy = view.CreatedBy,
                        CreatedDated = DateTime.Now,
                        Basicsalary = view.Basicsalary
                    };
                    _employeeRepository.Add(position);
                    Save();
                    view.EmployeeCode = position.EmployeeCode;
                    return view;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(EmployeeModelView view)
        {
            try
            {
                var dataServer = _employeeRepository.FindAll(p => p.EmployeeCode == view.EmployeeCode).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.PositionCode = view.PositionCode;
                    dataServer.RoomCode = view.RoomCode;
                    //dataServer.Avatar = view.Avatar;
                    dataServer.FullName = view.FullName;
                    dataServer.SDT = view.SDT;
                    dataServer.Email = view.Email;
                    dataServer.CMND = view.CMND;
                    dataServer.LevelAcademic = view.LevelAcademic;
                    dataServer.Gender = view.Gender;
                    dataServer.YearOfBirth = view.YearOfBirth;
                    dataServer.Address = view.Address;
                    //dataServer.IsDeleted = view.IsDeleted;
                    dataServer.UpdatedBy = view.UpdatedBy;
                    dataServer.UpdatedDated = DateTime.Now;
                    dataServer.Basicsalary = view.Basicsalary;
                    _employeeRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public void UpdateAvatar(string employeeCode,string avatar)
        {
            if (!string.IsNullOrEmpty(employeeCode) && !string.IsNullOrEmpty(avatar))
            {
                var dataServer = _employeeRepository.FindAll(p => p.EmployeeCode == employeeCode).FirstOrDefault();
                dataServer.Avatar = avatar;
                _employeeRepository.Update(dataServer);
                Save();
            }
        }
        public bool Deleted(string id)
        {
            try
            {
                var dataServer = _employeeRepository.FindAll(p => p.EmployeeCode == id).FirstOrDefault();
                if (dataServer != null)
                {
                   
                    dataServer.IsDeleted = true;
                    _employeeRepository.Update(dataServer);
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
        //public List<EmployeeModelView> GetAll()
        //{
        //    var data = _employeeRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new EmployeeModelView()
        //    {
        //        Id = c.Id,
        //        ProjectName = !string.IsNullOrEmpty(c.ProjectName) ? c.ProjectName : "",
        //        Status = c.Status,
        //        Note = c.Note
        //    }).ToList();
        //    return data;
        //}
        public PagedResult<EmployeeModelView> GetAllPaging(EmployeeModelViewSearch EmployeeModelViewSearch)
        {
            try
            {
                var query = _employeeRepository.FindAll(p=>p.IsDeleted != true);
                if (EmployeeModelViewSearch.IsSearch.HasValue && EmployeeModelViewSearch.IsSearch.Value == true)
                {
                    query = _employeeRepository.FindAll();
                }

                if (!string.IsNullOrEmpty(EmployeeModelViewSearch.Name))
                {
                    query = query.Where(c => c.FullName.ToLower().Trim().Contains(EmployeeModelViewSearch.Name.ToLower().Trim()) ||
                    c.Email.ToLower().Trim().Contains(EmployeeModelViewSearch.Name.ToLower().Trim()) ||
                    c.SDT.ToLower().Trim().Contains(EmployeeModelViewSearch.Name.ToLower().Trim()) ||
                    c.EmployeeCode.ToLower().Trim().Contains(EmployeeModelViewSearch.Name.ToLower().Trim()));
                }
                
                if (EmployeeModelViewSearch.PositionCode.HasValue && EmployeeModelViewSearch.PositionCode.Value > 0)
                {
                    query = query.Where(c => c.PositionCode == EmployeeModelViewSearch.PositionCode.Value);
                }
                if (EmployeeModelViewSearch.Status.HasValue && EmployeeModelViewSearch.Status.Value > 0)
                {
                    if (EmployeeModelViewSearch.Status.Value  == 1)
                    {
                        query = query.Where(c => c.IsDeleted.HasValue && c.IsDeleted.Value !=true);
                    }
                    else
                    {
                        query = query.Where(c => c.IsDeleted.HasValue && c.IsDeleted.Value == true);
                    }
                    
                }

                int totalRow = query.Count();
                query = query.Skip((EmployeeModelViewSearch.PageIndex - 1) * EmployeeModelViewSearch.PageSize).Take(EmployeeModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.EmployeeCode).Select(c => new EmployeeModelView()
                {
                    EmployeeCode = c.EmployeeCode,
                    PositionCode = c.PositionCode,                    
                    RoomCode = c.RoomCode,                    
                    Avatar = c.Avatar,
                    FullName = c.FullName,
                    SDT = c.SDT,
                    Email = c.Email,
                    CMND = c.CMND,
                    LevelAcademic = c.LevelAcademic,
                    LevelAcademicStr = c.LevelAcademic.HasValue ? Common.GetGender(c.LevelAcademic.Value) : "",
                    Gender = c.Gender,
                    GenderStr = c.Gender.HasValue ? Common.GetGender(c.Gender.Value) : "",
                    YearOfBirth = c.YearOfBirth,
                    YearOfBirthStr = c.YearOfBirth.HasValue ? c.YearOfBirth.Value.ToString("yyyy-MM-dd") : "",
                    Address = c.Address,
                    IsDeletedStr = c.IsDeleted.HasValue ? Common.GetEmployStatus(c.IsDeleted.Value) : "",
                    IsDeleted = c.IsDeleted,
                    CreatedBy = c.CreatedBy,
                    CreatedDated = c.CreatedDated,
                    CreatedDatedStr = c.CreatedDated.HasValue ? c.CreatedDated.Value.ToString("yyyy-MM-dd") : "",
                    UpdatedBy = c.UpdatedBy,
                    UpdatedDated = c.UpdatedDated,
                    Basicsalary = c.Basicsalary,
                    UpdatedDatedStr = c.UpdatedDated.HasValue ? c.UpdatedDated.Value.ToString("yyyy-MM-dd") : "",
                }).ToList();

                var pagingData = new PagedResult<EmployeeModelView>
                {
                    Results = data,
                    CurrentPage = EmployeeModelViewSearch.PageIndex,
                    PageSize = EmployeeModelViewSearch.PageSize,
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
