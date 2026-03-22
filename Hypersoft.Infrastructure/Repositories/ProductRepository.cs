using System.Runtime.CompilerServices;
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

    public async Task<bool> UpdateAsync(string id, Product product)
    {
        var result = await _context.Products.ReplaceOneAsync(p => p.id == id, product);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _context.Products.DeleteOneAsync(p => p.id == id);
        return result.DeletedCount > 0;
    }
    public async Task<IEnumerable<Product>> GetByNameAsync(string name)
    {
        return await _context.Products.Find(p => p.nome.Contains(name)).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoria_id)
    {
        return await _context.Products.Find(p => p.categoria_id == categoria_id).ToListAsync();
    }


}
