using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepo;
        private readonly IProductCategoryRepository productCategoryRepo;
        private readonly ICategoryRepository categoryRepo;

        public ProductService(
            IProductRepository productRepo,
            IProductCategoryRepository productCategoryRepo,
            ICategoryRepository categoryRepo)
        {
            this.productRepo = productRepo;
            this.productCategoryRepo = productCategoryRepo;
            this.categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await productRepo.GetAllAsync();
        }

        public async Task<Product?> GetAsync(int id)
        {
            var all = await productRepo.GetAllAsync();
            return all.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product item)
        {
            return await productRepo.AddAsync(item);
        }

        public async Task<Product?> UpdateAsync(Product item)
        {
            // 1️⃣ Controleer of voorraad geldig is
            if (item.Stock < 0)
            {
                // Zet op 0 of gooi een exception — jouw keuze
                item.Stock = 0;
                Console.WriteLine("⚠️ Voorraad was negatief en is hersteld naar 0.");
            }

            try
            {
                // 2️⃣ Update uitvoeren
                return await productRepo.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fout bij updaten product: {ex.Message}");
                return null; // veilig teruggeven bij fout
            }
        }


        public async Task<Product?> DeleteAsync(Product item)
        {
            return await productRepo.DeleteAsync(item);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var productIds = await productCategoryRepo.GetProductIdsByCategoryIdAsync(categoryId);
            var allProducts = await productRepo.GetAllAsync();
            return allProducts.Where(p => productIds.Contains(p.Id));
        }

        public async Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(int productId)
        {
            var allLinks = await productCategoryRepo.GetAllAsync();
            var categoryIds = allLinks
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .Distinct();

            var allCategories = await categoryRepo.GetAllAsync();
            return allCategories.Where(c => categoryIds.Contains(c.Id));
        }
    }
}
