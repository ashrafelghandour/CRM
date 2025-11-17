using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Auth;
using Application.Interfaces;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOfferingRepository, OfferingRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOfferingService, OfferingService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}