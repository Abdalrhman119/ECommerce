using Domain.Contracts;
using Domain.Models;
using Persistence.Data;

namespace Persistence.Repositaries
{
    public class UnitOfWork(StoreDbContext _storeDbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, Object> _repositaries = [];
        public IGenericRepositary<TEntity, TKey> GetRepositary<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (_repositaries.ContainsKey(type))
                return (GenericRepositary<TEntity, TKey>)_repositaries[type];

            var repo = new GenericRepositary<TEntity, TKey>(_storeDbContext);
            _repositaries[type] = repo;
            return repo;
        }

        public async Task<int> SaveChanges()
        {
            return await _storeDbContext.SaveChangesAsync();
        }
    }
}
