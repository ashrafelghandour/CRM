using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public interface IScheduleRepository
{
    Task<Schedule?> GetByIdAsync(int id);
    Task<IEnumerable<Schedule>> GetAllAsync();
    Task<IEnumerable<Schedule>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Schedule>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Schedule>> GetUpcomingAsync();
    Task<Schedule> AddAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Schedule schedule);
    Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
}

public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Schedule>> GetByUserIdAsync(int userId)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(s => s.CreatedBy)
            .Include(s => s.AssignedTo)
            .Include(s => s.Customer)
            .Where(s => s.IsActive && 
                       (s.CreatedById == userId || s.AssignedToId == userId) &&
                       s.StartTime >= now)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> GetByCustomerIdAsync(int customerId)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(s => s.CreatedBy)
            .Include(s => s.AssignedTo)
            .Include(s => s.Customer)
            .Where(s => s.IsActive && s.CustomerId == customerId && s.StartTime >= now)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> GetUpcomingAsync()
    {
        var now = DateTime.UtcNow;
        var oneWeekFromNow = now.AddDays(7);
        
        return await _dbSet
            .Include(s => s.CreatedBy)
            .Include(s => s.AssignedTo)
            .Include(s => s.Customer)
            .Where(s => s.IsActive && 
                       s.StartTime >= now && 
                       s.StartTime <= oneWeekFromNow)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Schedule>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.CreatedBy)
            .Include(s => s.AssignedTo)
            .Include(s => s.Customer)
            .Where(s => s.IsActive)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public override async Task<Schedule?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.CreatedBy)
            .Include(s => s.AssignedTo)
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
      return await _dbSet.Where(sch=>sch.StartTime ==startDate && sch.EndTime ==endDate ).ToListAsync();
    }
}