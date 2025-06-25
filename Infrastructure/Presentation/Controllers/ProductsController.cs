using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared;
using Shared.DTO;
using Shared.DTO.Products;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ProductResponse>>> GetAllProducts([FromQuery] ProductQueryParameters productQueryParameters)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(productQueryParameters);
            return Ok(products);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ProductResponse> GetProductById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return product;
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }

    }
}
