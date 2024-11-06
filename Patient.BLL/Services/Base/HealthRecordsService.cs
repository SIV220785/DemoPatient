using AutoMapper;
using Patient.BLL.Interfaces;
using Patient.DAL.Interfaces;

namespace Patient.BLL.Services.Base
{
    public abstract class HealthRecordsService<TEntity> : IHealthRecordsService<TEntity>
        where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        protected HealthRecordsService(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _unitOfWork = unitOfWork;

            _repository = (IGenericRepository<TEntity>)unitOfWork.GetType().GetProperty(typeof(TEntity).Name + "s")
                ?.GetValue(unitOfWork);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _repository.GetAllAsync();

        public virtual async Task<TEntity> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new InvalidOperationException("Entity not found");
            }
        }
    }
}
