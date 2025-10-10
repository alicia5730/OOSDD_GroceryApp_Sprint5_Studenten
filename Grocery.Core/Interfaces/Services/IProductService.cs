using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetAsync(int id);
        Task<Product> AddAsync(Product item);
        Task<Product?> UpdateAsync(Product item);
        Task<Product?> DeleteAsync(Product item);

        // Optional if you want filtering by category
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);

        // Optional if you want the reverse lookup
        Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(int productId);
    }
}