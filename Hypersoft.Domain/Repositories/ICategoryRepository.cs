using Hypersoft.Domain.Entities;

namespace Hypersoft.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category category);
    Task<Category?> GetByIdAsync(string id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}
