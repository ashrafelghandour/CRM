using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;
using System.Reflection;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<SuperAdmin> SuperAdmins => Set<SuperAdmin>();
    public DbSet<SystemUser> SystemUsers => Set<SystemUser>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Offering> Offerings => Set<Offering>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<CustomerFeedback> CustomerFeedbacks => Set<CustomerFeedback>();
    public DbSet<Schedule> Schedules => Set<Schedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        ConfigureGlobalFilters(modelBuilder);
    }


    private static void ConfigureGlobalFilters(ModelBuilder modelBuilder)
    {
     //   modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
        modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsActive);
        modelBuilder.Entity<Offering>().HasQueryFilter(o => o.IsActive);
        modelBuilder.Entity<Ticket>().HasQueryFilter(t => t.IsActive);
        modelBuilder.Entity<TicketComment>().HasQueryFilter(tc => tc.IsActive);
        modelBuilder.Entity<CustomerFeedback>().HasQueryFilter(cf => cf.IsActive);
        modelBuilder.Entity<Schedule>().HasQueryFilter(s => s.IsActive);
    }

   
}