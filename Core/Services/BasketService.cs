using AutoMapper;
using Domain.Contracts;
using Domain.Models.Baskets;
using Shared.DTO.Basket;
using ServicesAbstraction;
using Domain.ExceptionsAdd;

namespace Services
{
    public class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task DeleteAsync(string Id) =>
            await _basketRepository.DeleteAsync(Id);

        public async Task<BasketDto> GetAsync(string Id)
        {
            var basket = await _basketRepository.GetAsync(Id) ?? throw new BasketNotFoundException(Id);

            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);

            var UpdatedBasket = await _basketRepository.CreateOrUpdate(basket)
                                ?? throw new Exception("Can't Create Basket");

            return _mapper.Map<BasketDto>(UpdatedBasket);

        }
    }
}