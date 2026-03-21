using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using Hypersoft.Infrastructure.Data;
using MongoDB.Driver;

namespace Hypersoft.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MongoDbContext _context;

    public ProductRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _context.Products.Find(p => p.id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }
}
