
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class CategoryService: ICategoryService
    {
        
        private readonly ICategoryRepository repo;

        public CategoryService(ICategoryRepository repo)
        {
            this.repo = repo;
        }
        
        public Task<IEnumerable<Category>> GetAllAsync() => repo.GetAllAsync();
        public Task<Category?> GetByIdAsync(int id) => repo.GetByIdAsync(id);
    }
}


