using RedisDemo.Models;

namespace RedisDemo.Services
{
    public interface IProductService
    {
        public Response AddProduct(AddProductRequest addProduct);
        public Response GetProduct();
        public Response UpdateProduct(Product updateProduct);
        public Response DeleteProduct(Guid Id);
    }
}
