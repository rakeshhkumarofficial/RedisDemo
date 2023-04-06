using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
/*using Newtonsoft.Json;*/
using RedisDemo.Models;
using RedisDemo.SessionFactory;
using System.Text;
using System.Text.Json.Serialization;

namespace RedisDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly INHibernateSessionFactory _sessionFactory;
        private readonly IDistributedCache _distributedCache;
        private string KeyName = "GetProducts";
        Response response = new Response();
        public ProductService(INHibernateSessionFactory sessionFactory, IDistributedCache distributedCache)
        {
            _sessionFactory = sessionFactory;
            _distributedCache = distributedCache;
        }

        public Response AddProduct(AddProductRequest addProduct)
        {

            if(addProduct.Name == null || addProduct.Name == "")
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.Message = "Please Enter the product Name";
                response.Data = null;
                return response;
            }
            if (addProduct.Price == null || addProduct.Name == "")
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.Message = "Please Enter the product Price";
                response.Data = null;
                return response;
            }
            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = addProduct.Name,
                Price = addProduct.Price,
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(newProduct);
                transaction.Commit();
                response.IsSuccess = true;
                response.StatusCode = 200;
                response.Message = "Product Added";
                response.Data = newProduct;
                return response;
            }        
        }

        public Response GetProduct()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                response.IsSuccess = true;
                response.StatusCode = 200;
                response.Message = "All Product";
                string serializedList = string.Empty;
                var EncodedList = _distributedCache.Get(KeyName);
                if(EncodedList != null)
                {
                    response.Data = new List<Product>();
                    serializedList = Encoding.UTF8.GetString(EncodedList);
                    response.Data = JsonConvert.DeserializeObject<List<Product>>(serializedList);
                }
                else
                {
                    response.Data = session.QueryOver<Product>().List();
                    if(response.IsSuccess)
                    {
                        serializedList = JsonConvert.SerializeObject(response.Data);
                        EncodedList = Encoding.UTF8.GetBytes(serializedList);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                            .SetAbsoluteExpiration(TimeSpan.FromHours(6));
                        _distributedCache.Set(KeyName, EncodedList, options);
                    }
                }

                // response.Data = session.QueryOver<Product>().List();
                return response;
            }
        }
    }
}
