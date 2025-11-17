using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.DTOs.Offerings;

namespace WebAPI.Controllers.Offerings;

public class OfferingsController : ApiControllerBase
{
    private readonly IOfferingService _offeringService;

    public OfferingsController(IOfferingService offeringService)
    {
        _offeringService = offeringService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOfferings()
    {
        try
        {
            var offerings = await _offeringService.GetAllOfferingsAsync();
            var offeringResponses = offerings.Select(offering => new OfferingResponse
            {
                Id = offering.Id,
                Name = offering.Name,
                Description = offering.Description,
                Price = offering.Price,
                StartDate = offering.StartDate,
                EndDate = offering.EndDate,
                IsActive = offering.IsActive,
                ProductId = offering.ProductId,
                ProductName = offering.Product?.Name,
                CreatedBy = offering.CreatedBy,
                CreatedAt = offering.CreatedAt
            });

            return Ok(new { 
                success = true, 
                data = offeringResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve offerings: {ex.Message}");
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveOfferings()
    {
        try
        {
            var offerings = await _offeringService.GetActiveOfferingsAsync();
            var offeringResponses = offerings.Select(offering => new OfferingResponse
            {
                Id = offering.Id,
                Name = offering.Name,
                Description = offering.Description,
                Price = offering.Price,
                StartDate = offering.StartDate,
                EndDate = offering.EndDate,
                IsActive = offering.IsActive,
                ProductId = offering.ProductId,
                ProductName = offering.Product?.Name,
                CreatedBy = offering.CreatedBy,
                CreatedAt = offering.CreatedAt
            });

            return Ok(new { 
                success = true, 
                data = offeringResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve active offerings: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferingById(int id)
    {
        try
        {
            var offering = await _offeringService.GetOfferingByIdAsync(id);
            if (offering == null)
                return HandleError("Offering not found", 404);

            var offeringResponse = new OfferingResponse
            {
                Id = offering.Id,
                Name = offering.Name,
                Description = offering.Description,
                Price = offering.Price,
                StartDate = offering.StartDate,
                EndDate = offering.EndDate,
                IsActive = offering.IsActive,
                ProductId = offering.ProductId,
                ProductName = offering.Product?.Name,
                CreatedBy = offering.CreatedBy,
                CreatedAt = offering.CreatedAt
            };

            return HandleResult(offeringResponse, "Offering retrieved successfully");
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve offering: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffering([FromBody] CreateOfferingRequest request)
    {
        try
        {
            // Only SuperAdmin and Salesman can create offerings
            var userRole = GetUserRole();
            if (userRole != UserRole.SuperAdmin.ToString() && userRole != UserRole.Salesman.ToString())
                return HandleError("Access denied", 403);

            var createdBy = GetUserEmail();
            var offering = await _offeringService.CreateOfferingAsync(request,createdBy);

            var offeringResponse = new OfferingResponse
            {
                Id = offering.Id,
                Name = offering.Name,
                Description = offering.Description,
                Price = offering.Price,
                StartDate = offering.StartDate,
                EndDate = offering.EndDate,
                IsActive = offering.IsActive,
                ProductId = offering.ProductId,
                ProductName = offering.Product?.Name,
                CreatedBy = offering.CreatedBy,
                CreatedAt = offering.CreatedAt
            };

            return CreatedAtAction(nameof(GetOfferingById), new { id = offering.Id }, new { 
                success = true, 
                message = "Offering created successfully", 
                data = offeringResponse 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create offering: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOffering(int id, [FromBody] OfferingResponse request)
    {
        try
        {
            // Only SuperAdmin and Salesman can update offerings
            var userRole = GetUserRole();
            if (userRole != UserRole.SuperAdmin.ToString() && userRole != UserRole.Salesman.ToString())
                return HandleError("Access denied", 403);

            var existingOffering = await _offeringService.GetOfferingByIdAsync(id);
            if (existingOffering == null)
                return HandleError("Offering not found", 404);

            existingOffering.Name = request.Name;
            existingOffering.Description = request.Description;
            existingOffering.Price = request.Price;
            existingOffering.StartDate = request.StartDate;
            existingOffering.EndDate = request.EndDate;
            existingOffering.IsActive = request.IsActive;
            existingOffering.ProductId = request.ProductId;

            var result = await _offeringService.UpdateOfferingAsync(existingOffering);
            if (!result)
                return HandleError("Failed to update offering");

            return Ok(new { 
                success = true, 
                message = "Offering updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update offering: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOffering(int id)
    {
        try
        {
            // Only SuperAdmin can delete offerings
            if (GetUserRole() != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var result = await _offeringService.DeleteOfferingAsync(id);
            if (!result)
                return HandleError("Offering not found", 404);

            return Ok(new { 
                success = true, 
                message = "Offering deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete offering: {ex.Message}");
        }
    }
}