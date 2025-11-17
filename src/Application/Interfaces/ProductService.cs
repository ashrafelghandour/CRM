using Domain.Entities;
using Application.DTOs.Products;
using Application.DTOs.Offerings;

namespace Application.Interfaces;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> CreateProductAsync(CreateProductRequest request, string createdBy);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> UpdateProductStockAsync(int productId, int newStockQuantity);

}


public interface IOfferingService
{Task<Offering?> GetOfferingByIdAsync(int id);
    Task<IEnumerable<Offering>> GetAllOfferingsAsync();
    Task<IEnumerable<Offering>> GetActiveOfferingsAsync();
    Task<Offering> CreateOfferingAsync(CreateOfferingRequest request, string createdBy);
    Task<bool> UpdateOfferingAsync(Offering offering);
    Task<bool> DeleteOfferingAsync(int id);
    Task<bool> DeactivateExpiredOfferingsAsync();

}
public interface IScheduleService
{
    Task<Schedule?> GetScheduleByIdAsync(int id);
    Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
    Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(int userId);
    Task<IEnumerable<Schedule>> GetSchedulesByCustomerAsync(int customerId);
    Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync(int userId, string userRole);
    Task<Schedule> CreateScheduleAsync(Schedule schedule);
    Task<bool> UpdateScheduleAsync(Schedule schedule);
    Task<bool> DeleteScheduleAsync(int id);
    Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
}