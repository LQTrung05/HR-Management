
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
    public interface IProjectService
    {
        PagedResult<ProjectModelView> GetAllPaging(SearchModelView ProjectModelViewSearch);
        ProjectModelView GetByid(int id);
        List<ProjectModelView> GetAll();
        void Add(ProjectModelView view);
        bool Update(ProjectModelView view);
        bool Deleted(int id);
        void Save();
        bool IsExist(string name);
        int GetCount();
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private IUnitOfWork _unitOfWork;
        public ProjectService(IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }
        public int GetCount()
        {
            int count = 0;
            count = _projectRepository.FindAll().Count();
            return count;
        }
        public bool IsExist(string name)
        {
            var data = _projectRepository.FindAll().Any(p => p.ProjectName == name);
            return data;
        }
        public ProjectModelView GetByid(int id)
        {
            var data = _projectRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ProjectModelView()
                {
                    Id = data.Id,
                    ProjectName = !string.IsNullOrEmpty(data.ProjectName) ?data.ProjectName : "",
                    StartDate = data.StartDate,
                    EndDate= data.EndDate,
                    Status = data.Status,
                    StartDateStr = data.StartDate.HasValue ? data.StartDate.Value.ToString("yyyy-MM-dd") : "",
                    EndDateStr = data.EndDate.HasValue ? data.EndDate.Value.ToString("yyyy-MM-dd") : "",
                    StatusStr = data.Status.HasValue && data.Status.Value > 0 ? Common.GetStatus(data.Status.Value) : "",
                    Note = !string.IsNullOrEmpty(data.Note) ? data.Note : ""
                };
                return model;
            }
            return null;
        }
        public void Add(ProjectModelView view)
        {
            try
            {
                if (view != null)
                {
                    var position = new Project
                    {
                        ProjectName = !string.IsNullOrEmpty(view.ProjectName) ? view.ProjectName : "",
                        StartDate = view.StartDate,
                        EndDate = view.EndDate,
                        Status = 1,
                        Note = !string.IsNullOrEmpty(view.Note) ? view.Note : ""         
                    };
                    _projectRepository.Add(position);
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
        public bool Update(ProjectModelView view)
        {
            try
            {
                var dataServer = _projectRepository.FindAll(p=>p.Id == view.Id).FirstOrDefault();
                if (dataServer != null)
                {
                    if (dataServer.ProjectName.ToLower().Trim() != view.ProjectName.ToLower().Trim())
                    {
                        if (IsExist(view.ProjectName))
                        {
                            return false;
                        }
                    }
                    dataServer.ProjectName = !string.IsNullOrEmpty(view.ProjectName) ? view.ProjectName : "";
                    dataServer.StartDate = view.StartDate;
                    dataServer.EndDate = view.EndDate;
                    dataServer.Status = view.Status;
                    dataServer.Note = !string.IsNullOrEmpty(view.Note) ? view.Note : "";
                    _projectRepository.Update(dataServer);
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
                var dataServer = _projectRepository.FindAll(p=>p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _projectRepository.Update(dataServer);
                    //_projectRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<ProjectModelView> GetAll()
        {
            var data = _projectRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new ProjectModelView()
            {
                Id = c.Id,
                ProjectName = !string.IsNullOrEmpty(c.ProjectName) ? c.ProjectName : "",
                Status = c.Status,
                Note = c.Note
            }).ToList();
            return data;
        }
        public PagedResult<ProjectModelView> GetAllPaging(SearchModelView ProjectModelViewSearch)
        {
            try
            {
                var query = _projectRepository.FindAll();

                if (!string.IsNullOrEmpty(ProjectModelViewSearch.name))
                {
                    query = query.Where(c => c.ProjectName.ToLower().Trim().Contains(ProjectModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((ProjectModelViewSearch.PageIndex - 1) * ProjectModelViewSearch.PageSize).Take(ProjectModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new ProjectModelView()
                {
                    Id = c.Id,
                    ProjectName = !string.IsNullOrEmpty(c.ProjectName) ? c.ProjectName : "",
                    StartDate = c.StartDate,
                    StartDateStr = c.StartDate.HasValue ? c.StartDate.Value.ToString("yyyy-MM-dd") :"",
                    EndDate = c.EndDate,
                    EndDateStr = c.EndDate.HasValue ? c.EndDate.Value.ToString("yyyy-MM-dd") :"",
                    Status = c.Status,
                    StatusStr = c.Status.HasValue && c.Status.Value > 0 ? Common.GetStatus(c.Status.Value):"",
                    IsDeleted = c.IsDeleted,
                    IsDeletedStr = c.IsDeleted.HasValue && c.IsDeleted.Value ?"Đã xóa":"",
                    Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
                }).ToList();

                var pagingData = new PagedResult<ProjectModelView>
                {
                    Results = data,
                    CurrentPage = ProjectModelViewSearch.PageIndex,
                    PageSize = ProjectModelViewSearch.PageSize,
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
