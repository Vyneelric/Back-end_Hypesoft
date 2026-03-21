using Hypersoft.Domain.Entities;

namespace Hypersoft.Domain.Repositories;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(string id);
    Task<IEnumerable<Product>> GetAllAsync();
}
