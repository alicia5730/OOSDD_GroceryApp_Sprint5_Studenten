using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductRepository
    {
        // Sync methods (for existing code)
        List<Product> GetAll();
        Product? Get(int id);
        Product Add(Product item);
        Product? Delete(Product item);
        Product? Update(Product item);

        // Async versions (for services that use async/await)
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetAsync(int id);
        Task<Product> AddAsync(Product item);
        Task<Product?> UpdateAsync(Product item);
        Task<Product?> DeleteAsync(Product item);
    }
}