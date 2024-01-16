using Microsoft.EntityFrameworkCore;
using PowerTree.Maui.Interfaces;
using PowerTree.Maui.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //protected readonly PODContext _context;
        protected readonly PTContext _context;
        #region AsyncMethods
        //public async Task<List<T>> GetAllAsync()
        //{
        //    return await _context.Set<T>().ToListAsync();
        //}
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync();
        }
        //public Task AddAsync(T entity)
        //{
        //    return await _context.Set<T>().AddAsync(entity);
        //}


        #endregion

        #region Non Async Methods
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        // =====================  MOVE TO SPECIFICATION PATTERN ?? ===================================
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().AsNoTracking().Where(expression);
        }

        public IEnumerable<T> FindWithSpecificationPattern(ISpecification<T> specification = null)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }


        // ===============================================================
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }
        public T GetById(int id)
        {
            var result = _context.Set<T>().Find(id);
            _context.ChangeTracker.Clear();

            return result;
        }
        // ===============================================================

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }



        #endregion


        #region Xtors
        public GenericRepository(PTContext context)
        {
            _context = context;
        }


        #endregion
    }
}
