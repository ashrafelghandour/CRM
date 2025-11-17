using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public interface IOfferingRepository
{
    Task<Offering?> GetByIdAsync(int id);
    Task<IEnumerable<Offering>> GetAllAsync();
    Task<IEnumerable<Offering>> GetActiveAsync();
    Task<Offering> AddAsync(Offering offering);
    Task UpdateAsync(Offering offering);
    Task DeleteAsync(Offering offering);
}

public class OfferingRepository : BaseRepository<Offering>, IOfferingRepository
{
    public OfferingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Offering>> GetActiveAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(o => o.Product)
            .Where(o => o.IsActive && o.StartDate <= now && o.EndDate >= now)
            .OrderBy(o => o.Name)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Offering>> GetAllAsync()
    {
        return await _dbSet
            .Include(o => o.Product)
            .Where(o => o.IsActive)
            .OrderBy(o => o.Name)
            .ToListAsync();
    }

    public override async Task<Offering?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);
    }
}