using Hypersoft.Domain.Entities;

namespace Hypersoft.Domain.Repositories;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(string id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<bool> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<Product>> GetByNameAsync(string name);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoria_id);

}
