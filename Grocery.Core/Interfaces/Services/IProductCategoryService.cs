using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        // (Optional, if you need these later)
        // 🔹 Get all product-category relationships
        Task<IEnumerable<ProductCategory>> GetAllAsync();

        // 🔹 Get all categories that a specific product belongs to
        Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(int productId);
    }
}

