using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Offerings)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }
}