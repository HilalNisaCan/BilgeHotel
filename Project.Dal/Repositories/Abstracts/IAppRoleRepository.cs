﻿using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {
        Task<AppRole?> GetByNameAsync(string roleName);
    }
}
