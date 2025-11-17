using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

    public interface ICustomerRepository
    {
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task<bool> EmailExistsAsync(string email);
    }
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context):base(context)
    {
        
    }
    public async Task<bool>  EmailExistsAsync(string email)
    {
                return await _dbSet.AnyAsync(c => c.Email == email && c.IsActive);

    }
    public override async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _dbSet.Where(c => c.IsActive ).OrderBy(c=>c.Name).ToListAsync();
    }
}
