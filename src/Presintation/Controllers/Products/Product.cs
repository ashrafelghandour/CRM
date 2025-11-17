using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.DTOs;
using Application.DTOs.Products;

namespace WebAPI.Controllers.Products;

public class ProductsController : ApiControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            var productResponses = products.Select(product => new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt
            });

            return Ok(new { 
                success = true, 
                data = productResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve products: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return HandleError("Product not found", 404);

            var productResponse = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt
            };

            return HandleResult(productResponse, "Product retrieved successfully");
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve product: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            // Only SuperAdmin and Salesman can create products
            var userRole = GetUserRole();
            if (userRole != UserRole.SuperAdmin.ToString() && userRole != UserRole.Salesman.ToString())
                return HandleError("Access denied", 403);

            var createdBy = GetUserEmail();
            var product = await _productService.CreateProductAsync(request, createdBy);

            var productResponse = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, new { 
                success = true, 
                message = "Product created successfully", 
                data = productResponse 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create product: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductResponse request)
    {
        try
        {
            // Only SuperAdmin and Salesman can update products
            var userRole = GetUserRole();
            if (userRole != UserRole.SuperAdmin.ToString() && userRole != UserRole.Salesman.ToString())
                return HandleError("Access denied", 403);

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return HandleError("Product not found", 404);

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.price = request.Price;
            existingProduct.StockQuantity = request.StockQuantity;
            existingProduct.Category = request.Category;

            var result = await _productService.UpdateProductAsync(existingProduct);
            if (!result)
                return HandleError("Failed to update product");

            return Ok(new { 
                success = true, 
                message = "Product updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update product: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            // Only SuperAdmin can delete products
            if (GetUserRole() != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return HandleError("Product not found", 404);

            return Ok(new { 
                success = true, 
                message = "Product deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete product: {ex.Message}");
        }
    }
}