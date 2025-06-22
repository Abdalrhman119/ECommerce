using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTO.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")] //BaseUrl/api/Products
    [ApiController]
    public class BasketsController(IServiceManager _serviceManager) : ControllerBase
    {
        //1)Get User Basket

        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await _serviceManager.BasketService.GetAsync(id);
            return Ok(basket);
        }



        //2)Update User Basket
        //2.1) Crete Basket
        //2.2) Add Item To Basket
        //2.3) Remove Item From Basket
        //2.4) Update Basket Items Quantity +/-

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.UpdateAsync(basketDto);
            return Ok(basket);
        }



        //3)Delete User Basket : After Checkout => Empty Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            await _serviceManager.BasketService.DeleteAsync(id);
            return NoContent();
        }

    }
}
