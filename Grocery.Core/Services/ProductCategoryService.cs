using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository repo;
        private readonly IProductRepository productRepo;

        public ProductCategoryService(IProductCategoryRepository repo, IProductRepository productRepo)
        {
            this.repo = repo;
            this.productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            try
            {
                // 1️⃣ Haal product-IDs op van de categorie
                var productIds = await repo.GetProductIdsByCategoryIdAsync(categoryId) ?? new List<int>();

                // 2️⃣ Haal alle producten op
                var allProducts = await productRepo.GetAllAsync() ?? new List<Product>();

                // 3️⃣ Filter de producten op basis van ID’s
                var matchingProducts = allProducts.Where(p => productIds.Contains(p.Id));
                return matchingProducts;
            }
            catch (Exception ex)
            {
                // 4️⃣ Log of vang de fout op, maar laat de app niet crashen
                Console.WriteLine($"[ProductCategoryService] Fout bij ophalen producten: {ex.Message}");
                return new List<Product>(); // Geef een lege lijst terug in plaats van crash
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await repo.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(int productId)
        {
            var allLinks = await repo.GetAllAsync();
            var categoryIds = allLinks
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .Distinct();

            // Optional: only if you have CategoryRepository
            // var allCategories = await categoryRepo.GetAllAsync();
            // return allCategories.Where(c => categoryIds.Contains(c.Id));

            // For now, return empty list (so compiler stays happy)
            return new List<Category>();
        }
    }
}