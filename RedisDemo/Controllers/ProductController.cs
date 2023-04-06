using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using RedisDemo.Models;
using RedisDemo.Services;

namespace RedisDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var res = _productService.GetProduct();
            return Ok(res);
        }

        [HttpPost]
        public IActionResult Post(AddProductRequest addProduct)
        {
            var res = _productService.AddProduct(addProduct);
            return Ok(res);
        }
    }
}
