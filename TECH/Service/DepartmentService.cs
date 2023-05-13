
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
    public interface IDepartmentService
    {
        PagedResult<DepartmentModelView> GetAllPaging(SearchModelView DepartmentModelViewSearch);
        DepartmentModelView GetByid(int id);
        List<DepartmentModelView> GetAll();
        void Add(DepartmentModelView view);
        bool Update(DepartmentModelView view);
        bool Deleted(int id);
        void Save();
        bool IsExist(string name);
        int GetCount();
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private IUnitOfWork _unitOfWork;
        public DepartmentService(IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        public int GetCount()
        {
            int count = 0;
            count = _departmentRepository.FindAll().Count();
            return count;
        }

        /// <summary>
        /// Check tên phòng ban tồn tại hay chưa
        /// </summary>
        /// <param name="name">Tên phòng ban truyền vào</param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            var data = _departmentRepository.FindAll().Any(p => p.RoomName == name);
            return data;
        }
        public DepartmentModelView GetByid(int id)
        {
            var data = _departmentRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new DepartmentModelView()
                {
                    Id = data.Id,
                    RoomName = !string.IsNullOrEmpty(data.RoomName) ?data.RoomName : "",
                    Note = !string.IsNullOrEmpty(data.Note) ? data.Note : ""
                };
                return model;
            }
            return null;
        }
        public void Add(DepartmentModelView view)
        {
            try
            {
                if (view != null)
                {
                    var project = new Department
                    {
                        RoomName = !string.IsNullOrEmpty(view.RoomName) ? view.RoomName : "",
                        Note = !string.IsNullOrEmpty(view.Note) ? view.Note : ""
                    };
                    _departmentRepository.Add(project);
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
        public bool Update(DepartmentModelView view)
        {
            try
            {
                var dataServer = _departmentRepository.FindAll(p=>p.Id == view.Id).FirstOrDefault();
                if (dataServer != null)
                {
                    if (dataServer.RoomName.ToLower().Trim() != view.RoomName.ToLower().Trim())
                    {
                        if (IsExist(view.RoomName))
                        {
                            return false;
                        }
                    }
                    dataServer.RoomName = !string.IsNullOrEmpty(view.RoomName) ? view.RoomName : "";
                    dataServer.Note = !string.IsNullOrEmpty(view.Note) ? view.Note : "";
                    _departmentRepository.Update(dataServer);
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
                var dataServer = _departmentRepository.FindAll(p=>p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    _departmentRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<DepartmentModelView> GetAll()
        {
            var data = _departmentRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new DepartmentModelView()
            {
                Id = c.Id,
                RoomName = !string.IsNullOrEmpty(c.RoomName) ? c.RoomName : "",
                Note = c.Note
            }).ToList();
            return data;
        }
        public PagedResult<DepartmentModelView> GetAllPaging(SearchModelView DepartmentModelViewSearch)
        {
            try
            {
                var query = _departmentRepository.FindAll();

                if (!string.IsNullOrEmpty(DepartmentModelViewSearch.name))
                {
                    query = query.Where(c => c.RoomName.ToLower().Trim().Contains(DepartmentModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((DepartmentModelViewSearch.PageIndex - 1) * DepartmentModelViewSearch.PageSize).Take(DepartmentModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new DepartmentModelView()
                {
                    Id = c.Id,
                    RoomName = !string.IsNullOrEmpty(c.RoomName) ? c.RoomName : "",
                    Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
                }).ToList();

                var pagingData = new PagedResult<DepartmentModelView>
                {
                    Results = data,
                    CurrentPage = DepartmentModelViewSearch.PageIndex,
                    PageSize = DepartmentModelViewSearch.PageSize,
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
