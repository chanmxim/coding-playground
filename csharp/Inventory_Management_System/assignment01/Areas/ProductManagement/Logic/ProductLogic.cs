using assignment01.Areas.OrderManagement.Models;
using assignment01.Areas.ProductManagement.Models;
using assignment01.Data;
using assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.ProductManagement.Logic;

public class ProductLogic
{
    private readonly ApplicationDbContext _context;
    private readonly ErrorLog _errorLog;

    public ProductLogic(ApplicationDbContext context, ErrorLog errorLog)
    {
        _context = context;
        _errorLog = errorLog;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<Product?> GetProductWithCategoryByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return product;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }


    public async Task<List<Product>> GetAllLowStockProductsListAsync()
    {
        try
        {
            var products = await _context.Products
                .Where(p => p.Quantity <= p.LowStockThreshold)
                .ToListAsync();

            return products;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public IQueryable<Product> ApplySearchByNameAndCategoryToProductsQuery(IQueryable<Product> productsQuery, string searchString)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            productsQuery = productsQuery.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower()) || p.Category.CategoryName.ToLower().Contains(searchString.ToLower()));
        }

        return productsQuery;
    }

    public IQueryable<Product> ApplySortToProductsQuery(IQueryable<Product> productsQuery, string sortType)
    {
        switch (sortType)
        {
            case "nameDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.ProductName);
                break;
            case "priceAsc":
                productsQuery = productsQuery.OrderBy(p => p.Price);
                break;
            case "priceDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
                break;
            case "quantityAsc":
                productsQuery = productsQuery.OrderBy(p => p.Quantity);
                break;
            case "quantityDesc":
                productsQuery = productsQuery.OrderByDescending(p => p.Quantity);
                break;
            default:
                productsQuery = productsQuery.OrderBy(p => p.ProductName);
                break;
        }

        return productsQuery;
    }

    public IQueryable<Product> ApplyFilterToProductsQuery(IQueryable<Product> productsQuery,
        List<int>? categories,
        double? minPrice, double? maxPrice,
        bool? lowStockStatus)
    {
        productsQuery = ApplyCategoryFilter(productsQuery, categories);
        productsQuery = ApplyPriceFilter(productsQuery, minPrice, maxPrice);
        productsQuery = ApplyLowStockStatusFilter(productsQuery, lowStockStatus);

        return productsQuery;
    }

    private IQueryable<Product> ApplyCategoryFilter(IQueryable<Product> productsQuery, List<int>? categories)
    {
        if (categories != null && categories.Any())
        {
            productsQuery = productsQuery.Where(p => categories.Contains(p.CategoryId));
        }

        return productsQuery;
    }

    private IQueryable<Product> ApplyPriceFilter(IQueryable<Product> productsQuery, double? minPrice, double? maxPrice)
    {
        if (minPrice != null && maxPrice != null && minPrice.Value < maxPrice.Value)
        {
            productsQuery = productsQuery.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        return productsQuery;
    }

    private IQueryable<Product> ApplyLowStockStatusFilter(IQueryable<Product> productsQuery, bool? lowStockStatus)
    {
        if (lowStockStatus != null)
        {
            if (lowStockStatus == true)
            {
                productsQuery = productsQuery.Where(p => p.Quantity <= p.LowStockThreshold);
            }
        }

        return productsQuery;
    }

    public IQueryable<Product> CreateQueryToGetAllExistingProducts()
    {
        try
        {
            var productsQuery = _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category);
            return productsQuery;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> DeleteProductAsync(Product product)
    {
        try
        {
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public bool ValidateProductId(int id, Product product)
    {
        return id == product.ProductId;
    }

    public async Task<int> GetTotalNumberOfStockAsync()
    {
        try
        {
            return await _context.Products.Where(p => p.IsDeleted == false).SumAsync(p => p.Quantity);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<List<Category>> GetAllCategoriesListAsync()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> BindCategoryObjectToProduct(Product product)
    {
        var category = await GetCategoryByIdAsync(product.CategoryId);
        product.Category = category;
        return true;
    }

    public bool BindOrderItemsObjectToProduct(Product product)
    {
        if (product.OrdersItems == null)
        {
            product.OrdersItems = new List<OrderItem>();
        }

        return true;
    }
}