
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
    public interface IBonusPunishService
    {
        BonusPunishModelView GetByid(int id);
        List<BonusPunishModelView> GetAll();
        List<BonusPunishModelView> GetByPayRollId(int payrollId);
        List<BonusPunishModelView> GetByPayRollByEmploye(string employeeCode);
        BonusPunishModelView Add(BonusPunishModelView view);
        bool Update(BonusPunishModelView view);
        List<BonusPunishModelView> GetByForEmployee(string employee);
        bool Deleted(int id);
        void Save();
        //void DeletedByRollId(int maProject);
        void DeletedByEmpployeeCode(string maProject);
    }

    public class BonusPunishService : IBonusPunishService
    {
        private readonly IBonusPunishRepository _bonusPunishRepository;
        private IUnitOfWork _unitOfWork;
        public BonusPunishService(IBonusPunishRepository bonusPunishRepository,
            IUnitOfWork unitOfWork)
        {
            _bonusPunishRepository = bonusPunishRepository;
            _unitOfWork = unitOfWork;
        }
        //public void DeletedByRollId(int projectCode)
        //{
        //    if (projectCode > 0)
        //    {
        //        var dichVuPhongByMaPhong = _bonusPunishRepository.FindAll(d => d.PayrollId == projectCode).ToList();
        //        if (dichVuPhongByMaPhong != null && dichVuPhongByMaPhong.Count > 0)
        //        {
        //            foreach (var item in dichVuPhongByMaPhong)
        //            {
        //                Deleted(item.Id);
        //            }
        //            Save();
        //        }
        //    }
        //}
        public void DeletedByEmpployeeCode(string employeecode)
        {
            if (!string.IsNullOrEmpty(employeecode))
            {
                var dichVuPhongByMaPhong = _bonusPunishRepository.FindAll(d => d.EmployeeCode == employeecode).ToList();
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
        public BonusPunishModelView GetByid(int id)
        {
            var data = _bonusPunishRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new BonusPunishModelView()
                {
                    Id = data.Id,
                    //PayrollId = data.PayrollId,
                    RewardType = data.RewardType,
                    EmployeeCode = data.EmployeeCode,
                    RewardTypeStr = data.RewardType.HasValue && data.RewardType.Value > 0 ? Common.GetBonuesPunishStatus(data.RewardType.Value):"",
                    RewardTypeName = !string.IsNullOrEmpty(data.RewardTypeName) ? data.RewardTypeName : "",
                    PenaltyBonus = data.PenaltyBonus,
                    PenaltyBonusStr = data.PenaltyBonus.HasValue && data.PenaltyBonus.Value > 0 ? data.PenaltyBonus.Value.ToString("#,###") : "",
                    Note = !string.IsNullOrEmpty(data.Note) ? data.Note : ""
                };
                return model;
            }
            return null;
        }
        public List<BonusPunishModelView> GetByPayRollId(int payrollId)
        {
            var data = _bonusPunishRepository.FindAll(p => p.Id == payrollId).Select(c => new BonusPunishModelView()
            {
                Id = c.Id,
                RewardType = c.RewardType,
                EmployeeCode = c.EmployeeCode,
                //PayrollId = c.PayrollId,
                RewardTypeStr = c.RewardType.HasValue && c.RewardType.Value > 0 ? Common.GetBonuesPunishStatus(c.RewardType.Value) : "",
                RewardTypeName = !string.IsNullOrEmpty(c.RewardTypeName) ? c.RewardTypeName : "",
                PenaltyBonus = c.PenaltyBonus,
                PenaltyBonusStr = c.PenaltyBonus.HasValue && c.PenaltyBonus.Value > 0 ? c.PenaltyBonus.Value.ToString("#,###") : "",
                Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
            }).ToList();
            
            return data;
        }
        public List<BonusPunishModelView> GetByForEmployee(string employee)
        {
            if (!string.IsNullOrEmpty(employee))
            {
                var data = _bonusPunishRepository.FindAll(p => p.EmployeeCode == employee.Trim()).Select(c => new BonusPunishModelView()
                {
                    Id = c.Id,
                    RewardType = c.RewardType,
                    EmployeeCode = c.EmployeeCode,
                    //PayrollId = c.PayrollId,
                    RewardTypeStr = c.RewardType.HasValue && c.RewardType.Value > 0 ? Common.GetBonuesPunishStatus(c.RewardType.Value) : "",
                    RewardTypeName = !string.IsNullOrEmpty(c.RewardTypeName) ? c.RewardTypeName : "",
                    PenaltyBonus = c.PenaltyBonus,
                    PenaltyBonusStr = c.PenaltyBonus.HasValue && c.PenaltyBonus.Value > 0 ? c.PenaltyBonus.Value.ToString("#,###") : "",
                    Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
                }).ToList();
                return data;
            }
            return null;
        }
        public List<BonusPunishModelView> GetByPayRollByEmploye(string employeeCode)
        {
            var data = _bonusPunishRepository.FindAll(p => p.EmployeeCode == employeeCode).Select(c => new BonusPunishModelView()
            {
                Id = c.Id,
                RewardType = c.RewardType,
                //PayrollId = c.PayrollId,
                RewardTypeStr = c.RewardType.HasValue && c.RewardType.Value > 0 ? Common.GetBonuesPunishStatus(c.RewardType.Value) : "",
                RewardTypeName = !string.IsNullOrEmpty(c.RewardTypeName) ? c.RewardTypeName : "",
                PenaltyBonus = c.PenaltyBonus,
                PenaltyBonusStr = c.PenaltyBonus.HasValue && c.PenaltyBonus.Value > 0 ? c.PenaltyBonus.Value.ToString("#,###") : "",
                Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
            }).ToList();

            return data;
        }
        public BonusPunishModelView Add(BonusPunishModelView view)
        {
            try
            {
                if (view != null)
                {
                    var project = new BonusPunish
                    {
                        RewardType = view.RewardType,
                        EmployeeCode = view.EmployeeCode,
                        //PayrollId = view.PayrollId,
                        RewardTypeName =  view.RewardTypeName,
                        PenaltyBonus = view.PenaltyBonus,                     
                        Note = !string.IsNullOrEmpty(view.Note) ? view.Note : ""
                    };
                    _bonusPunishRepository.Add(project);
                    Save();
                    view.Id = project.Id;
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
        public bool Update(BonusPunishModelView view)
        {
            try
            {
                var dataServer = _bonusPunishRepository.FindAll(p=>p.Id == view.Id).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.RewardType = view.RewardType;
                    //dataServer.PayrollId = view.PayrollId;
                    dataServer.EmployeeCode = view.EmployeeCode;
                    dataServer.RewardTypeName = view.RewardTypeName;
                    dataServer.PenaltyBonus = view.PenaltyBonus;
                    dataServer.Note = !string.IsNullOrEmpty(view.Note) ? view.Note : "";
                    _bonusPunishRepository.Update(dataServer);
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
                var dataServer = _bonusPunishRepository.FindAll(p=>p.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    _bonusPunishRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<BonusPunishModelView> GetAll()
        {
            var data = _bonusPunishRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new BonusPunishModelView()
            {
                Id = c.Id,
                RewardType = c.RewardType,
                EmployeeCode = c.EmployeeCode,
                //PayrollId = c.PayrollId,
                RewardTypeStr = c.RewardType.HasValue && c.RewardType.Value > 0 ? Common.GetBonuesPunishStatus(c.RewardType.Value) : "",
                RewardTypeName = !string.IsNullOrEmpty(c.RewardTypeName) ? c.RewardTypeName : "",
                PenaltyBonus = c.PenaltyBonus,
                PenaltyBonusStr = c.PenaltyBonus.HasValue && c.PenaltyBonus.Value > 0 ? c.PenaltyBonus.Value.ToString("#,###") : "",
                Note = !string.IsNullOrEmpty(c.Note) ? c.Note : ""
            }).ToList();
            return data;
        }
    }
}
