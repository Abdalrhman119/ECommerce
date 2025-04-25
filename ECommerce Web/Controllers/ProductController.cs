using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            return new Product() { Id = id };
        }
        [HttpGet]
        public ActionResult<Product> GetAll()
        {
            return new Product() { Id = 1 };
        }
        [HttpPost("add")]
        public ActionResult<Product> Add(Product product)
        {
            return new Product() { Id = product.Id ,Name=product.Name };
        }
        [HttpPost("update")]
        public ActionResult<Product> Update(Product product)
        {
            return new Product() { Id = product.Id, Name = product.Name };
        }
        [HttpPost("delete")]
        public ActionResult<Product> Delete(Product product)
        {
            return new Product() { Id = product.Id, Name = product.Name };
        }

    }
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    } 
}
