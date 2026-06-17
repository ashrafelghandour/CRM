using Domain.Entities;
using Application.DTOs.Offerings;

namespace Application.Interfaces;

public interface IOfferingService
{Task<Offering?> GetOfferingByIdAsync(int id);
    Task<IEnumerable<Offering>> GetAllOfferingsAsync();
    Task<IEnumerable<Offering>> GetActiveOfferingsAsync();
    Task<Offering> CreateOfferingAsync(CreateOfferingRequest request, string createdBy);
    Task<bool> UpdateOfferingAsync(Offering offering);
    Task<bool> DeleteOfferingAsync(int id);
    Task<bool> DeactivateExpiredOfferingsAsync();

}
