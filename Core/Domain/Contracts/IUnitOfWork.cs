using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
        //IGenericRepositary<Product, int> ProductRepositary { get; }
        //IGenericRepositary<ProductBrand, int> ProductBrandRepositary { get; }
        //IGenericRepositary<ProductType, int> ProductTypeRepositary { get; }
        IGenericRepositary<TEntity, TKey> GetRepositary<TEntity, TKey>() where TEntity : BaseEntity<TKey>;


    }
}
