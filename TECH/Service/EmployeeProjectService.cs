
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
    public interface IEmployeeProjectService
    {
        EmployeeProjectModelView GetByid(int id);
        EmployeeProjectModelView GetByThanhVienByMaProjectMaKH(string maTV, int maProject);
        void Add(EmployeeProjectModelView view);
        bool Update(EmployeeProjectModelView view);
        bool Deleted(int id);
        void DeletedByMaPhong(int maProject);
     
        void Save();
        List<EmployeeProjectModelView> GetThanhVienByMaProject(int maProject);
    }

    public class EmployeeProjectService : IEmployeeProjectService
    {
        private readonly IEmployeeProjectRepository _employeeProjectRepository;
     
        private IUnitOfWork _unitOfWork;
        public EmployeeProjectService(IEmployeeProjectRepository employeeProjectRepository,
           
        IUnitOfWork unitOfWork,
            IPositionRepository positionRepository)
        {
            _employeeProjectRepository = employeeProjectRepository;
          
            _unitOfWork = unitOfWork;
        }
      
        public EmployeeProjectModelView GetByThanhVienByMaProjectMaKH(string employeeCode, int projectCode)
        {
            if (!string.IsNullOrEmpty(employeeCode) && projectCode > 0)
            {
                var thanhvien = _employeeProjectRepository.FindAll(p => p.EmployeeCode == employeeCode && p.ProjectCode == projectCode).Select(p => new EmployeeProjectModelView()
                {
                    Id = p.Id,
                    EmployeeCode = p.EmployeeCode,
                    ProjectCode = p.ProjectCode,
                }).FirstOrDefault();

                if (thanhvien != null)
                    return thanhvien;
            }
            return null;
        }
        public EmployeeProjectModelView GetByid(int id)
        {
            var data = _employeeProjectRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new EmployeeProjectModelView()
                {
                    Id = data.Id,
                    EmployeeCode = data.EmployeeCode,
                    ProjectCode = data.ProjectCode,
                };
                return model;
            }
            return null;
        }

        public void Add(EmployeeProjectModelView view)
        {
            try
            {
                if (view != null)
                {
                    var dichVuPhong = new EmployeeProject
                    {
                        EmployeeCode = view.EmployeeCode,
                        ProjectCode = view.ProjectCode,
                    };
                    _employeeProjectRepository.Add(dichVuPhong);
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
        public bool Update(EmployeeProjectModelView view)
        {
            try
            {
                var dataServer = _employeeProjectRepository.FindAll(p=>p.Id == view.Id).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.EmployeeCode = view.EmployeeCode;
                    dataServer.ProjectCode = view.ProjectCode;
                    _employeeProjectRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _employeeProjectRepository.FindAll(p => p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    _employeeProjectRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }
        public void DeletedByMaPhong(int projectCode)
        {
            if (projectCode > 0)
            {
                var dichVuPhongByMaPhong = _employeeProjectRepository.FindAll(d => d.ProjectCode == projectCode).ToList();
                if (dichVuPhongByMaPhong != null && dichVuPhongByMaPhong.Count > 0)
                {
                    foreach (var item in dichVuPhongByMaPhong)
                    {
                        Deleted(item.Id);
                    }
                    Save();
                }
            }
        }
        public List<EmployeeProjectModelView> GetThanhVienByMaProject(int maProject)
        {
            var data = _employeeProjectRepository.FindAll(p => p.ProjectCode == maProject).Select(p => new EmployeeProjectModelView()
            {
                Id = p.Id,
                EmployeeCode = p.EmployeeCode,
                ProjectCode = p.ProjectCode,
            }).ToList();
            return data;
        }
    }
}
