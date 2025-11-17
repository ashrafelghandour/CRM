using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Products;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request, string createdBy)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            CreatedBy = createdBy
        };

        return await _productRepository.AddAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(product.Id);
        if (existingProduct == null) return false;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.price = product.price;
        existingProduct.StockQuantity = product.StockQuantity;
        existingProduct.Category = product.Category;

        await _productRepository.UpdateAsync(existingProduct);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }

    public async Task<bool> UpdateProductStockAsync(int productId, int newStockQuantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return false;

        product.StockQuantity = newStockQuantity;
        await _productRepository.UpdateAsync(product);
        return true;
    }
}