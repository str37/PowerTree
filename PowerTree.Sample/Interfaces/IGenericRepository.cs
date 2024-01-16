using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Sample.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region Non Async Methods
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);


        #endregion

        #region Async Methods
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();



        #endregion
    }
}
