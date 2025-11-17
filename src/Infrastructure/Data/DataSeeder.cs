using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;

    public DataSeeder(AppDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            // Check if any users exist (not just active ones)
            if (!await _context.Users.AnyAsync())
            {
                await SeedUsers();
                await _context.SaveChangesAsync();
            }

            if (!await _context.Customers.AnyAsync())
            {
                await SeedCustomers();
                await _context.SaveChangesAsync();
            }

            if (!await _context.Products.AnyAsync())
            {
                await SeedProducts();
                await _context.SaveChangesAsync();
            }

            if (!await _context.Offerings.AnyAsync())
            {
                await SeedOfferings();
                await _context.SaveChangesAsync();
            }

            if (!await _context.Tickets.AnyAsync())
            {
                await SeedTickets();
                await _context.SaveChangesAsync();
            }

            if (!await _context.Schedules.AnyAsync())
            {
                await SeedSchedules();
                await _context.SaveChangesAsync();
            }

            if (!await _context.CustomerFeedbacks.AnyAsync())
            {
                await SeedCustomerFeedbacks();
                await _context.SaveChangesAsync();
            }

            Console.WriteLine("✅ All data seeded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error during seeding: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    private async Task SeedUsers()
    {
        var users = new List<User>
        {
            new SuperAdmin
            {
                Username = "superadmin",
                Email = "superadmin@company.com",
                PassworedHash = _passwordService.HashPassword("Admin123!"),
                FirstName = "Ahmed",
                LastName = "Ali",
                Phone = "+201234567890",
                Role = UserRole.SuperAdmin,
                CreatedAt = DateTime.UtcNow.AddMonths(-12)
                ,IsActive=true
            },
            new SystemUser
            {
                Username = "salesman1",
                Email = "mohamed.sales@company.com",
                PassworedHash = _passwordService.HashPassword("Sales123!"),
                FirstName = "Mohamed",
                LastName = "Mahmoud",
                Phone = "+201234567891",
                Role = UserRole.Salesman,
                EmployeeId = "SAL001",
                Department = "Sales",
                HireDate = DateTime.UtcNow.AddMonths(-18),
                CreatedAt = DateTime.UtcNow.AddMonths(-18),IsActive=true
            },
            new SystemUser
            {
                Username = "salesman2",
                Email = "sara.sales@company.com",
                PassworedHash = _passwordService.HashPassword("Sales123!"),
                FirstName = "Sara",
                LastName = "Hassan",
                Phone = "+201234567892",
                Role = UserRole.Salesman,
                EmployeeId = "SAL002",
                Department = "Sales",
                HireDate = DateTime.UtcNow.AddMonths(-12),
                CreatedAt = DateTime.UtcNow.AddMonths(-12),IsActive=true
            },
            new SystemUser
            {
                Username = "employee1",
                Email = "khaled.support@company.com",
                PassworedHash = _passwordService.HashPassword("Employee123!"),
                FirstName = "Khaled",
                LastName = "Ibrahim",
                Phone = "+201234567893",
                Role = UserRole.Employee,
                EmployeeId = "EMP001",
                Department = "Technical Support",
                HireDate = DateTime.UtcNow.AddMonths(-9),
                CreatedAt = DateTime.UtcNow.AddMonths(-9),IsActive=true
            },
            new SystemUser
            {
                Username = "employee2",
                Email = "fatima.support@company.com",
                PassworedHash = _passwordService.HashPassword("Employee123!"),
                FirstName = "Fatima",
                LastName = "Osman",
                Phone = "+201234567894",
                Role = UserRole.Employee,
                EmployeeId = "EMP002",
                Department = "Customer Service",
                HireDate = DateTime.UtcNow.AddMonths(-6),
                CreatedAt = DateTime.UtcNow.AddMonths(-6),IsActive=true
            }
        };

        await _context.Users.AddRangeAsync(users);
        Console.WriteLine("✅ Users seeded: 5 users added");
    }

    private async Task SeedCustomers()
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                Name = "Tech Solutions Egypt",
                Email = "contact@techsolutions-eg.com",
                Phone = "+20224567890",
                Address = "15 Nile Street, Cairo, Egypt",
                CompanyName = "Tech Solutions Egypt",
                CreatedBy = "superadmin@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-10),IsActive=true
            },
            new Customer
            {
                Name = "Modern Trade Company",
                Email = "info@moderntrade.com",
                Phone = "+20334567891",
                Address = "25 Alexandria Road, Alexandria, Egypt",
                CompanyName = "Modern Trade Co.",
                CreatedBy = "mohamed.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-8),IsActive=true
            },
            new Customer
            {
                Name = "Delta Industries",
                Email = "sales@deltaindustries.com",
                Phone = "+20444567892",
                Address = "10 Port Said Street, Mansoura, Egypt",
                CompanyName = "Delta Industries LLC",
                CreatedBy = "sara.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-6),IsActive=true
            },
            new Customer
            {
                Name = "Nile Business Group",
                Email = "info@nilebusiness.com",
                Phone = "+20554567893",
                Address = "5 Tahrir Square, Giza, Egypt",
                CompanyName = "Nile Business Group",
                CreatedBy = "mohamed.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-4),IsActive=true
            },
            new Customer
            {
                Name = "Red Sea Trading",
                Email = "contact@redseatrading.com",
                Phone = "+20664567894",
                Address = "30 Hurghada Road, Suez, Egypt",
                CompanyName = "Red Sea Trading Co.",
                CreatedBy = "sara.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-2),IsActive=true
            }
        };

        await _context.Customers.AddRangeAsync(customers);
        Console.WriteLine("✅ Customers seeded: 5 customers added");
    }

    private async Task SeedProducts()
    {
        var products = new List<Product>
        {
            new Product
            {
                Name = "Enterprise Laptop Dell XPS",
                Description = "High-performance business laptop with 16GB RAM and 512GB SSD",
                price = 25000.00m,
                StockQuantity = 25,
                Category = "Electronics",
                CreatedBy = "superadmin@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-8),IsActive=true
            },
            new Product
            {
                Name = "Ergonomic Office Chair",
                Description = "Comfortable office chair with lumbar support and adjustable height",
                price = 3500.00m,
                StockQuantity = 50,
                Category = "Furniture",
                CreatedBy = "mohamed.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-7),IsActive=true
            },
            new Product
            {
                Name = "Professional Desk",
                Description = "Large executive desk with built-in cable management",
                price = 6000.00m,
                StockQuantity = 30,
                Category = "Furniture",
                CreatedBy = "sara.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-6),IsActive=true
            },
            new Product
            {
                Name = "Wireless Keyboard & Mouse Set",
                Description = "Premium wireless keyboard and mouse combo for office use",
                price = 1200.00m,
                StockQuantity = 100,
                Category = "Electronics",
                CreatedBy = "mohamed.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-5),IsActive=true
            },
            new Product
            {
                Name = "Conference Room Monitor",
                Description = "55-inch 4K monitor for conference room presentations",
                price = 15000.00m,
                StockQuantity = 15,
                Category = "Electronics",
                CreatedBy = "sara.sales@company.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-4),IsActive=true
            }
        };

        await _context.Products.AddRangeAsync(products);
        Console.WriteLine("✅ Products seeded: 5 products added");
    }

    private async Task SeedOfferings()
    {
        // Get products from database instead of context
        var products = await _context.Products.ToListAsync();
        
        if (products.Count >= 3)
        {
            var offerings = new List<Offering>
            {
                new Offering
                {
                    Name = "Summer Office Setup Package",
                    Description = "Complete office setup including desk, chair, and accessories",
                    Price = 12000.00m,
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(30),
                    IsActive = true,
                    ProductId = products[1].Id,
                    CreatedBy = "mohamed.sales@company.com",
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                },
                new Offering
                {
                    Name = "Tech Bundle Special",
                    Description = "Laptop + Monitor + Accessories bundle with 15% discount",
                    Price = 38000.00m,
                    StartDate = DateTime.UtcNow.AddDays(-15),
                    EndDate = DateTime.UtcNow.AddDays(45),
                    IsActive = true,
                    ProductId = products[0].Id,
                    CreatedBy = "sara.sales@company.com",
                    CreatedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new Offering
                {
                    Name = "Executive Desk Package",
                    Description = "Premium desk with ergonomic chair and cable management",
                    Price = 8000.00m,
                    StartDate = DateTime.UtcNow.AddDays(-60),
                    EndDate = DateTime.UtcNow.AddDays(-1),
                    IsActive = false,
                    ProductId = products[2].Id,
                    CreatedBy = "mohamed.sales@company.com",
                    CreatedAt = DateTime.UtcNow.AddMonths(-3)
                }
            };

            await _context.Offerings.AddRangeAsync(offerings);
            Console.WriteLine("✅ Offerings seeded: 3 offerings added");
        }
        else
        {
            Console.WriteLine("⚠️ Not enough products to create offerings");
        }
    }

    private async Task SeedTickets()
    {
        var customers = await _context.Customers.ToListAsync();
        var employees = await _context.SystemUsers.ToListAsync();
        var superAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.SuperAdmin);
        
        if (customers.Count >= 4 && employees.Count >= 2 && superAdmin != null)
        {
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Title = "Laptop not turning on",
                    Description = "The Dell XPS laptop we purchased last month is not turning on. No lights or signs of power.",
                    Status = TicketStatus.Resolved,
                    Priority = TicketPriority.High,
                    CustomerId = customers[0].Id,
                    AssignedToId = employees[0].Id,
                    CreatedById = superAdmin.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),IsActive=true
                },
                new Ticket
                {
                    Title = "Office chair adjustment problem",
                    Description = "The height adjustment mechanism on the ergonomic chair is not working properly.",
                    Status = TicketStatus.InProgress,
                    Priority = TicketPriority.Medium,
                    CustomerId = customers[1].Id,
                    AssignedToId = employees[1].Id,
                    CreatedById = customers[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),IsActive=true
                },
                new Ticket
                {
                    Title = "Monitor flickering issue",
                    Description = "The conference room monitor has flickering issues during presentations.",
                    Status = TicketStatus.Open,
                    Priority = TicketPriority.High,
                    CustomerId = customers[2].Id,
                    AssignedToId = employees[0].Id,
                    CreatedById = customers[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Ticket
                {
                    Title = "Request for additional products catalog",
                    Description = "We would like to receive your latest products catalog for our purchasing department.",
                    Status = TicketStatus.Open,
                    Priority = TicketPriority.Low,
                    CustomerId = customers[3].Id,
                    CreatedById = customers[3].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            await _context.Tickets.AddRangeAsync(tickets);
            
            // Add comments separately
            var ticket1 = tickets[0];
            var comments = new List<TicketComment>
            {
                new TicketComment
                {
                    Comment = "Thank you for reporting the issue. I've assigned this to our technical team.",
                    TicketId = ticket1.Id,
                    UserId = superAdmin.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new TicketComment
                {
                    Comment = "I've diagnosed the issue as a faulty power adapter. Replacement has been shipped.",
                    TicketId = ticket1.Id,
                    UserId = employees[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),IsActive=true
                }
            };

            await _context.TicketComments.AddRangeAsync(comments);
            Console.WriteLine("✅ Tickets seeded: 4 tickets with comments added");
        }
        else
        {
            Console.WriteLine("⚠️ Not enough data to create tickets");
        }
    }

    private async Task SeedSchedules()
    {
        var customers = await _context.Customers.ToListAsync();
        var employees = await _context.SystemUsers.ToListAsync();
        var superAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.SuperAdmin);
        
        if (customers.Count >= 3 && employees.Count >= 2 && superAdmin != null)
        {
            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    Title = "Product Demo - Tech Solutions",
                    Description = "Demonstration of new conference room equipment",
                    StartTime = DateTime.UtcNow.AddDays(3).AddHours(10),
                    EndTime = DateTime.UtcNow.AddDays(3).AddHours(12),
                    Location = "Tech Solutions Egypt Office, Cairo",
                    CreatedById = employees[0].Id,
                    AssignedToId = employees[1].Id,
                    CustomerId = customers[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),IsActive=true
                },
                new Schedule
                {
                    Title = "Quarterly Business Review",
                    Description = "Quarterly performance review and future planning",
                    StartTime = DateTime.UtcNow.AddDays(7).AddHours(14),
                    EndTime = DateTime.UtcNow.AddDays(7).AddHours(16),
                    Location = "Modern Trade Company, Alexandria",
                    CreatedById = superAdmin.Id,
                    AssignedToId = employees[0].Id,
                    CustomerId = customers[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Schedule
                {
                    Title = "Installation Support Visit",
                    Description = "On-site installation and setup of purchased equipment",
                    StartTime = DateTime.UtcNow.AddDays(5).AddHours(9),
                    EndTime = DateTime.UtcNow.AddDays(5).AddHours(13),
                    Location = "Delta Industries, Mansoura",
                    CreatedById = employees[0].Id,
                    AssignedToId = employees[0].Id,
                    CustomerId = customers[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),IsActive=true
                }
            };

            await _context.Schedules.AddRangeAsync(schedules);
            Console.WriteLine("✅ Schedules seeded: 3 schedules added");
        }
        else
        {
            Console.WriteLine("⚠️ Not enough data to create schedules");
        }
    }

    private async Task SeedCustomerFeedbacks()
    {
        var customers = await _context.Customers.ToListAsync();
        
        if (customers.Count >= 4)
        {
            var feedbacks = new List<CustomerFeedback>
            {
                new CustomerFeedback
                {
                    Feedbacks = "Excellent service and support. The technical team was very professional and solved our issue quickly.",
                    Rating = 5,
                    FeedbackDate = DateTime.UtcNow.AddDays(-15),
                    CustomerId = customers[0].Id,IsActive=true
                },
                new CustomerFeedback
                {
                    Feedbacks = "Good products but delivery was delayed by 2 days. Overall satisfied with the quality.",
                    Rating = 4,
                    FeedbackDate = DateTime.UtcNow.AddDays(-10),
                    CustomerId = customers[1].Id
                },
                new CustomerFeedback
                {
                    Feedbacks = "The sales team was very helpful in choosing the right products for our office. Great experience!",
                    Rating = 5,
                    FeedbackDate = DateTime.UtcNow.AddDays(-7),
                    CustomerId = customers[3].Id,IsActive=true
                },
                new CustomerFeedback
                {
                    Feedbacks = "Product quality is good but customer service response time could be improved.",
                    Rating = 3,
                    FeedbackDate = DateTime.UtcNow.AddDays(-3),
                    CustomerId = customers[2].Id
                }
            };

            await _context.CustomerFeedbacks.AddRangeAsync(feedbacks);
            Console.WriteLine("✅ Customer feedbacks seeded: 4 feedbacks added");
        }
        else
        {
            Console.WriteLine("⚠️ Not enough customers to create feedbacks");
        }
    }
}