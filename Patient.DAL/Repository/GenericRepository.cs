using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Patient.DAL.Interfaces;

namespace Patient.DAL.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public IQueryable<TEntity> Include(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        return include(_dbSet);
    }

    public TEntity GetById(object id) => _dbSet.Find(id);

    public async Task<TEntity> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

    public void Add(TEntity entity) => _dbSet.Add(entity);

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        AttachIfDetached(entity);
        _dbSet.Remove(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Delete(entity);
        return Task.CompletedTask;
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        AttachIfDetached(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public Task UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Update(entity);
        return Task.CompletedTask;
    }

    private void AttachIfDetached(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
    }
}
