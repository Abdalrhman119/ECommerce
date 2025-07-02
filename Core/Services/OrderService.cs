using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ExceptionsAdd;
using Domain.Models.Baskets;
using Domain.Models.Orders;
using Domain.Models.Products;
using Services.Specifications;
using Microsoft.Extensions.Options;
using Services.Specifications;
using ServicesAbstraction;
using Shared.DTO.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(IBasketRepository _basketRepository,
                              IUnitOfWork _unitOfWork,
                              IMapper _mapper) : IOrderService
    {
        public async Task<OrderResponse> CreateAsync(OrderRequest orderRequest, string email)
        {
            var basket = await _basketRepository.GetAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);
            var orderRepo = _unitOfWork.GetRepositary<Order, Guid>(); 

            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetByIdAsync(specs);


            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
            }
            List<OrderItem> items = [];

            foreach (var item in basket.Items)
            {
                var OriginalProduct = await _unitOfWork.GetRepositary<Product, int>()
                                                       .GetByIdAsync(item.Id)
                                                       ?? throw new ProductNotFoundException(item.Id);

                items.Add(CreateOrderItem(OriginalProduct, item));
            }

            var method = await _unitOfWork.GetRepositary<DeliveryMethod, int>()
                                          .GetByIdAsync(orderRequest.DeliveryMethodId)
                                          ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var Address = _mapper.Map<OrderAddress>(orderRequest.ShipToAddress);

            var SubTotal = items.Sum(i => i.Price * i.Quantity);

            var Order = new Order(email, items, Address, SubTotal, method, basket.PaymentIntentId);
            //Order.DeliveryMethodId = method.Id;///////////
            orderRepo.Add(Order);
            await _unitOfWork.SaveChanges(); 

            //await _basketRepository.DeleteAsync(orderRequest.BasketId);

            return _mapper.Map<OrderResponse>(Order);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync(string email)
        {
            var orders = await _unitOfWork.GetRepositary<Order, Guid>()
                                          .GetAllAsync(new OrderSpecification(email));

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> GetByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.GetRepositary<Order, Guid>()
                                          .GetByIdAsync(new OrderSpecification(orderId));

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepositary<DeliveryMethod, int>()
                                             .GetAllAsync();

            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(DeliveryMethods);
        }

        private OrderItem CreateOrderItem(Product originalProduct, BasketItem item)
        {
            return new OrderItem()
            {
                ProductName = originalProduct.Name,
                PictureUrl = originalProduct.PictureUrl,
                Price = originalProduct.Price,
                Quantity = item.Quantity,
                ProductId = originalProduct.Id
            };
        }
    }
}