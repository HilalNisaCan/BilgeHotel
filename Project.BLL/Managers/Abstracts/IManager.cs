﻿using Project.BLL.DtoClasses;
using Project.Entities.Interfaces;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IManager<T, U> where T : BaseDto where U : class, IEntity
    {
        //Business Logic For Queries

        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        List<T> GetActives();
        List<T> GetPassives();
        List<T> GetModifieds();
        List<T> Where(Expression<Func<U, bool>> exp);

        //Business Logic for Commands
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<string> RemoveAsync(T entity);
        Task MakePassiveAsync(T entity);

        Task CreateRangeAsync(List<T> list);
        Task UpdateRangeAsync(List<T> list);
        Task<string> RemoveRangeAsync(List<T> list);
      
    }
}
