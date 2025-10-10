

using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories =
        [
            new Category(1, "Zuivel"),
            new Category(2, "Bakkerij"),
            new Category(3, "Ontbijtgranen"),
        ];

        public Task<IEnumerable<Category>> GetAllAsync() =>
            Task.FromResult(_categories.AsEnumerable());
        
        public Task<Category?> GetByIdAsync(int id) =>
            Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
    }
}

