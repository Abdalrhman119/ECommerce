using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepositary<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);


        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();


        Task<TEntity> GetByIdAsync(ISpecifications<TEntity> specifications);

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications);

    }
}
