﻿using AutoMapper;
using Domain.Models.Orders;
using Microsoft.Extensions.Configuration;
using Shared.Authentication;
using Shared.DTO.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Cost))
                .ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost));
            CreateMap<DeliveryMethod, DeliveryMethodResponse>();
        }



    }

    public class OrderItemPictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }


}