using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
    }
}

