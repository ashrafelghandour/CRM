using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Customers;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer> CreateCustomerAsync(CustomerRequest request, string createdBy)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CompanyName = request.CompanyName,
            CreatedBy = createdBy
        };

        return await _customerRepository.AddAsync(customer);
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id);
        if (existingCustomer == null) return false;

        existingCustomer.Name = customer.Name;
        existingCustomer.Email = customer.Email;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.Address = customer.Address;
        existingCustomer.CompanyName = customer.CompanyName;
        existingCustomer.UpdateAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(existingCustomer);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;

        await _customerRepository.DeleteAsync(customer);
        return true;
    }

    
}