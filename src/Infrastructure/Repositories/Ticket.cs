using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<IEnumerable<Ticket>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Ticket>> GetByAssigneeIdAsync(int assigneeId);
    Task<Ticket> AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task AddCommentAsync(TicketComment comment);
    Task DeleteAsync(Ticket ticket);

}

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ticket>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(t => t.Customer)
            .Include(t => t.AssignedTo)
            .Include(t => t.Comments)
            .Where(t => t.CustomerId == customerId && t.IsActive)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByAssigneeIdAsync(int assigneeId)
    {
        return await _dbSet
            .Include(t => t.Customer)
            .Include(t => t.AssignedTo)
            .Where(t => t.AssignedToId == assigneeId && t.IsActive)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public override async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _dbSet
        .Include(t => t.Customer)
            .Include(t => t.AssignedTo)
            .Include(t => t.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
        
    }    

    public async Task AddCommentAsync(TicketComment comment)
    {
        await _context.TicketComments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public override Task DeleteAsync(Ticket entity)
    {
        return base.DeleteAsync(entity);
    }

   
}