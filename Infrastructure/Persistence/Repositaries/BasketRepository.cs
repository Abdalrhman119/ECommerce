using Domain.Contracts;
using Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositaries
{
    public class BasketRepository : IBasketRepository
    {
        public Task<CustomerBasket?> CreateOrUpdate(CustomerBasket basket)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerBasket?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
