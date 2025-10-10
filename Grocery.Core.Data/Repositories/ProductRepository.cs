using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> products;

        public ProductRepository()
        {
            products = new List<Product>
            {
                new Product(1, "Melk", 300, new DateOnly(2025, 9, 25), 1.29),
                new Product(2, "Kaas", 100, new DateOnly(2025, 9, 30), 4.79),
                new Product(3, "Brood", 400, new DateOnly(2025, 9, 12), 2.39),
                new Product(4, "Cornflakes", 0, new DateOnly(2025, 12, 31), 3.49)
            };
        }

        public List<Product> GetAll() => products;

        public Product? Get(int id) => products.FirstOrDefault(p => p.Id == id);

        public Product Add(Product item)
        {
            products.Add(item);
            return item;
        }

        public Product? Delete(Product item)
        {
            products.Remove(item);
            return item;
        }

        public Product? Update(Product item)
        {
            var existing = products.FirstOrDefault(p => p.Id == item.Id);
            if (existing == null) return null;
            existing.Name = item.Name;
            existing.Stock = item.Stock;
            existing.Price = item.Price;
            return existing;
        }

        // Async versions
        public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult<IEnumerable<Product>>(products);
        public Task<Product?> GetAsync(int id) => Task.FromResult(Get(id));
        public Task<Product> AddAsync(Product item) => Task.FromResult(Add(item));
        public Task<Product?> UpdateAsync(Product item) => Task.FromResult(Update(item));
        public Task<Product?> DeleteAsync(Product item) => Task.FromResult(Delete(item));
    }
}