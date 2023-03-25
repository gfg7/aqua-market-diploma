using AquaServer.Interfaces.Models;
using AquaServer.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AquaServer.Interfaces.Database
{
    public interface IRepository<T> where T : IEntity, new()
    {
        Task<T> GetById(IComparable id);
        Task<IQueryable<T>> Get(bool track = true);
        Task<T> Update(T update);
        Task<T> Create(T model);
        Task<T> Delete(IComparable id);
    }
}
