using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using Hypersoft.Infrastructure.Data;
using MongoDB.Driver;

namespace Hypersoft.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MongoDbContext _context;

    public CategoryRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.InsertOneAsync(category);
        return category;
    }

    public async Task<Category?> GetByIdAsync(string id)
    {
        return await _context.Categories.Find(c => c.id == id).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.Find(_ => true).ToListAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _context.Categories.DeleteOneAsync(c => c.id == id);
        return result.DeletedCount > 0;
    }
}
