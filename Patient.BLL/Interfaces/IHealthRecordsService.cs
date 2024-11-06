namespace Patient.BLL.Interfaces;

public interface IHealthRecordsService<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(int id);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task DeleteAsync(int id);
}
