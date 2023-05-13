
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
    public interface IPositionService
    {
        PagedResult<PositionModelView> GetAllPaging(SearchModelView PositionModelViewSearch);
        PositionModelView GetByid(int id);
        List<PositionModelView> GetAll();
        void Add(PositionModelView view);
        bool Update(PositionModelView view);
        bool Deleted(int id);
        void Save();
        bool IsExist(string name);
        int GetCount();
    }

    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private IUnitOfWork _unitOfWork;
        public PositionService(IPositionRepository positionRepository,
            IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsExist(string tenDichVu)
        {
            var data = _positionRepository.FindAll().Any(p => p.PositionName == tenDichVu);
            return data;
        }
        public int GetCount()
        {
            int count = 0;
            count = _positionRepository.FindAll().Count();
            return count;
        }
        public PositionModelView GetByid(int id)
        {
            var data = _positionRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new PositionModelView()
                {
                    Id = data.Id,
                    PositionName = !string.IsNullOrEmpty(data.PositionName)? data.PositionName:"",
                    Coefficient = data.Coefficient,
                    Note = !string.IsNullOrEmpty(data.Note) ? data.Note : "",
                };
                return model;
            }
            return null;
        }
       
        public void Add(PositionModelView view)
        {
            try
            {
                if (view != null)
                {
                    var position = new Position
                    {
                        PositionName = view.PositionName,
                        Coefficient = view.Coefficient,
                        Note = view.Note                   
                    };
                    _positionRepository.Add(position);
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
        public bool Update(PositionModelView view)
        {
            try
            {
                var dataServer = _positionRepository.FindAll(p=>p.Id == view.Id).FirstOrDefault();
                if (dataServer != null)
                {
                    if (dataServer.PositionName.ToLower().Trim() != view.PositionName.ToLower().Trim())
                    {
                        if (IsExist(view.PositionName))
                        {
                            return false;
                        }
                    }
                    dataServer.PositionName = view.PositionName;
                    dataServer.Coefficient = view.Coefficient;
                    dataServer.Note = view.Note;
                    _positionRepository.Update(dataServer);
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
                var dataServer = _positionRepository.FindAll(p=>p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    _positionRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<PositionModelView> GetAll()
        {
            var data = _positionRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new PositionModelView()
            {
                Id = c.Id,
                PositionName = !string.IsNullOrEmpty(c.PositionName) ? c.PositionName : "",
                Coefficient = c.Coefficient,
                Note = c.Note
            }).ToList();
            return data;
        }
        public PagedResult<PositionModelView> GetAllPaging(SearchModelView PositionModelViewSearch)
        {
            try
            {
                var query = _positionRepository.FindAll();

                if (!string.IsNullOrEmpty(PositionModelViewSearch.name))
                {
                    query = query.Where(c => c.PositionName.ToLower().Trim().Contains(PositionModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((PositionModelViewSearch.PageIndex - 1) * PositionModelViewSearch.PageSize).Take(PositionModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new PositionModelView()
                {
                    Id = c.Id,
                    PositionName = !string.IsNullOrEmpty(c.PositionName) ? c.PositionName : "",
                    Coefficient = c.Coefficient,
                    Note = c.Note
                }).ToList();

                var pagingData = new PagedResult<PositionModelView>
                {
                    Results = data,
                    CurrentPage = PositionModelViewSearch.PageIndex,
                    PageSize = PositionModelViewSearch.PageSize,
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
