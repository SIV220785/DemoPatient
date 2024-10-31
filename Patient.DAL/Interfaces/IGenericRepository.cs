namespace Patient.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        TEntity GetById(object id);

        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);
    }
}
