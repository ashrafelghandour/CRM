using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Offerings;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class OfferingService : IOfferingService
{
    private readonly IOfferingRepository _offeringRepository;
    private readonly IProductRepository _productRepository;

    public OfferingService(IOfferingRepository offeringRepository, IProductRepository productRepository)
    {
        _offeringRepository = offeringRepository;
        _productRepository = productRepository;
    }

    public async Task<Offering?> GetOfferingByIdAsync(int id)
    {
        return await _offeringRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Offering>> GetAllOfferingsAsync()
    {
        return await _offeringRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Offering>> GetActiveOfferingsAsync()
    {
        return await _offeringRepository.GetActiveAsync();
    }

    public async Task<Offering> CreateOfferingAsync(CreateOfferingRequest request, string createdBy)
    {
        // Validate product exists if provided
        if (request.ProductId.HasValue)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId.Value);
            if (product == null)
                throw new ArgumentException("Product not found");
        }

        var offering = new Offering
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true,
            ProductId = request.ProductId,
            CreatedBy = createdBy
        };

        return await _offeringRepository.AddAsync(offering);
    }

    public async Task<bool> UpdateOfferingAsync(Offering offering)
    {
        var existingOffering = await _offeringRepository.GetByIdAsync(offering.Id);
        if (existingOffering == null) return false;

        // Validate product exists if provided
        if (offering.ProductId.HasValue && offering.ProductId.Value != existingOffering.ProductId)
        {
            var product = await _productRepository.GetByIdAsync(offering.ProductId.Value);
            if (product == null)
                throw new ArgumentException("Product not found");
        }

        existingOffering.Name = offering.Name;
        existingOffering.Description = offering.Description;
        existingOffering.Price = offering.Price;
        existingOffering.StartDate = offering.StartDate;
        existingOffering.EndDate = offering.EndDate;
        existingOffering.IsActive = offering.IsActive;
        existingOffering.ProductId = offering.ProductId;

        await _offeringRepository.UpdateAsync(existingOffering);
        return true;
    }

    public async Task<bool> DeleteOfferingAsync(int id)
    {
        var offering = await _offeringRepository.GetByIdAsync(id);
        if (offering == null) return false;

        await _offeringRepository.DeleteAsync(offering);
        return true;
    }

    public async Task<bool> DeactivateExpiredOfferingsAsync()
    {
        var expiredOfferings = await _offeringRepository.GetAllAsync();
        var now = DateTime.UtcNow;
        var updated = false;

        foreach (var offering in expiredOfferings.Where(o => o.EndDate < now && o.IsActive))
        {
            offering.IsActive = false;
            await _offeringRepository.UpdateAsync(offering);
            updated = true;
        }

        return updated;
    }
}