namespace RedisDemo.Models
{
    public class Product
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; } 
        public virtual decimal Price { get; set; }
    }
}
