using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IProjectRepository : IRepository<Project>
    {
       
    }

    public class ProjectRepository : EFRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
