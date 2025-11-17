using Application.DTOs.Customers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICustomerService
{
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> CreateCustomerAsync(CustomerRequest request, string createdBy);
    Task<bool> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(int id);
}
}