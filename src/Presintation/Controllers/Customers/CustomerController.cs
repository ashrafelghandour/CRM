using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs.Customers;
namespace WebAPI.Controllers.Customers;

public class CustomersController : ApiControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
    {
            var customers = await _customerService.GetAllCustomersAsync();
            var customerResponses = customers.Select(customer => new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                CompanyName = customer.CompanyName,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy
            });

            return Ok(new { 
                success = true, 
                data = customerResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve customers: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return HandleError("Customer not found", 404);

            var customerResponse = new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                CompanyName = customer.CompanyName,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy
            };

            return HandleResult(customerResponse, "Customer retrieved successfully");
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve customer: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest request)
    {
        try
        {
            var createdBy = GetUserEmail();
            var customer = await _customerService.CreateCustomerAsync(request, createdBy);

            var customerResponse = new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                CompanyName = customer.CompanyName,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, new { 
                success = true, 
                message = "Customer created successfully", 
                data = customerResponse 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create customer: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerResponse request)
    {
        try
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
                return HandleError("Customer not found", 404);

            existingCustomer.Name = request.Name;
            existingCustomer.Email = request.Email;
            existingCustomer.Phone = request.Phone;
            existingCustomer.Address = request.Address;
            existingCustomer.CompanyName = request.CompanyName;

            var result = await _customerService.UpdateCustomerAsync(existingCustomer);
            if (!result)
                return HandleError("Failed to update customer");

            return Ok(new { 
                success = true, 
                message = "Customer updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update customer: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return HandleError("Customer not found", 404);

            return Ok(new { 
                success = true, 
                message = "Customer deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete customer: {ex.Message}");
        }
    }
}