using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAccountRepository : IRepository<Account>
    {
       
    }

    public class AccountRepository : EFRepository<Account>, IAccountRepository
    {
        public AccountRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
