using AquaServer.Extensions.Exceptions;
using AquaServer.Interfaces.Database;
using AquaServer.Interfaces.Models;
using AquaServer.Models.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AquaServer.Domain.Database
{
    public class Repository<T> : IRepository<T> where T : Base, IEntity, new()
    {
        private readonly AquamContext _context;

        public Repository(AquamContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T model)
        {
            var obj = await _context.Set<T>().AddAsync(model);
            await _context.SaveChangesAsync();
            return obj.Entity;
        }

        public async Task<T> Delete(IComparable id)
        {
            var obj = await GetById(id);
            _context.Set<T>().Remove(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<IQueryable<T>> Get(bool track=true)
        {
            var result = _context.Set<T>().AsQueryable();

            if (!track)
            {
                result = result.AsNoTracking();
            }

            return result;
        }

        public async Task<T> GetById(IComparable id)
        {
            var obj = await _context.Set<T>().FindAsync(id);

            if (obj is null)
                throw new EntityNotFoundException();

            return obj;
        }

        public async Task<T> Update(T model)
        {
            var obj = _context.Set<T>().Update(model).Entity;
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
