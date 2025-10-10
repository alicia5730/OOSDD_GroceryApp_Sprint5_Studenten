using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<IEnumerable<int>> GetProductIdsByCategoryIdAsync(int categoryId);
    }
}

