using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Patient.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Include(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);

        TEntity GetById(object id);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> GetByIdAsync(object id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);
    }
}
