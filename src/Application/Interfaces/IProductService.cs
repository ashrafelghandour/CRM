using Domain.Entities;
using Application.DTOs.Products;

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
