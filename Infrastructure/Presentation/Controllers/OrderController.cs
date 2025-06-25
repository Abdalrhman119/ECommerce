using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTO.Orders;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IServiceManager _serviceManager) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Create(OrderRequest request)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await _serviceManager.OrderService.CreateAsync(request, email);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]

        public async Task<ActionResult<OrderResponse>> GetById(Guid orderId)
        {
            var order = await _serviceManager.OrderService.GetByIdAsync(orderId);
            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderService.GetAllAsync(email);
            return Ok(orders);
        }

        [HttpGet("DeliveryMethods")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResponse>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}