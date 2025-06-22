using AutoMapper;
using Domain.Models.Baskets;
using Shared.DTO.Basket;
using Shared.DTO.BasketAdd;

namespace Services.MappingProfileAdd
{
    public class BasketProfile : Profile
    {

        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }

    }
}