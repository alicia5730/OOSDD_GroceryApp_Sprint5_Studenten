using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        public Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            var list = new List<ProductCategory>
            {
                new ProductCategory(1, "Zuivel", 1, 1),
                new ProductCategory(2, "Zuivel", 2, 1),
                new ProductCategory(3, "Bakkerij", 3, 2),
                new ProductCategory(4, "Ontbijt", 4, 3),
            };

            return Task.FromResult<IEnumerable<ProductCategory>>(list);
        }

        public Task<IEnumerable<int>> GetProductIdsByCategoryIdAsync(int categoryId)
        {
            var all = new List<ProductCategory>
            {
                new ProductCategory(1, "Zuivel", 1, 1),
                new ProductCategory(2, "Zuivel", 2, 1),
                new ProductCategory(3, "Bakkerij", 3, 2),
                new ProductCategory(4, "Ontbijt", 4, 3)
            };

            var ids = all
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.ProductId);

            return Task.FromResult<IEnumerable<int>>(ids);
        }
    }
}
