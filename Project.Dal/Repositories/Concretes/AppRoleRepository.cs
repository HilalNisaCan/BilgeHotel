using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class AppRoleRepository : BaseRepository<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository(MyContext context) : base(context) { }

        public async Task<AppRole?> GetByNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
