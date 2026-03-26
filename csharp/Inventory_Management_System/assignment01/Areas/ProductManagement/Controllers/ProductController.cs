using assignment01.Areas.ProductManagement.Logic;
using assignment01.Areas.ProductManagement.Models;
using assignment01.Controllers;
using assignment01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.ProductManagement.Controllers;

[Route("Products")]
[Area("ProductManagement")]
[Authorize]
public class ProductController : BaseController
{
    private readonly ProductLogic _productLogic;
    private readonly ErrorLog _errorLog;

    // dependency injection
    public ProductController(ProductLogic productLogic, ErrorLog errorLog)
    {
        _productLogic = productLogic;
        _errorLog = errorLog;
    }

    /// <summary>
    /// Render the list of all products
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<IActionResult> Index(string sortType,
        List<int>? categories,
        double? minPrice, double? maxPrice,
        bool? lowStockStatus)
    {
        // Save filter and sorting configurations inside ViewBag
        ViewBag.SortType = String.IsNullOrWhiteSpace(sortType) ? "sortAsc" : sortType;
        ViewBag.MinPrice = minPrice == null ? "" : minPrice.Value.ToString("F0");
        ViewBag.MaxPrice = maxPrice == null ? "" : maxPrice.Value.ToString("F0");
        ViewBag.SelectedCategories = categories == null ? new List<int>() : categories;
        ViewBag.LowStockStatus = lowStockStatus == null ? "" : "true";

        ViewBag.Categories = await _productLogic.GetAllCategoriesListAsync();

        var productsQuery = _productLogic.CreateQueryToGetAllExistingProducts();

        productsQuery =
            _productLogic.ApplyFilterToProductsQuery(productsQuery, categories, minPrice, maxPrice, lowStockStatus);
        productsQuery = _productLogic.ApplySortToProductsQuery(productsQuery, sortType);

        try
        {
            var products = await productsQuery.ToListAsync();

            return View(products);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpGet("LiveProductsSearch")]
    public async Task<IActionResult> LiveProductsSearch(string searchString)
    {
        var productsQuery = _productLogic.CreateQueryToGetAllExistingProducts();

        productsQuery = _productLogic.ApplySearchByNameAndCategoryToProductsQuery(productsQuery, searchString);
        try
        {
            var products = await productsQuery.ToListAsync();

            return PartialView("Product/_ProductListPartial", products);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }


    /// <summary>
    /// Render add product form
    /// </summary>
    /// <returns></returns>
    [HttpGet("Add")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add()
    {
        ViewBag.CategoryList = await _productLogic.GetAllCategoriesListAsync();
        return View();
    }

    /// <summary>
    /// Redirect to Index if object is created successfully
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(Product product)
    {
        await _productLogic.BindCategoryObjectToProduct(product);
        _productLogic.BindOrderItemsObjectToProduct(product);

        ModelState.ClearValidationState("Category");
        ModelState.ClearValidationState("OrdersItems");
        TryValidateModel(product);

        if (ModelState.IsValid)
        {
            await _productLogic.AddProductAsync(product);
            return RedirectToAction("Index");
        }

        ViewBag.CategoryList = await _productLogic.GetAllCategoriesListAsync();
        return View(product);
    }

    /// <summary>
    /// Render edit page form
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Edit/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productLogic.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        ViewBag.CategoryList = await _productLogic.GetAllCategoriesListAsync();
        return View(product);
    }

    /// <summary>
    /// Redirect to Index if object is edited successfully
    /// </summary>
    /// <param name="id"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "ProductId,ProductName,ProductDescription,Price,Quantity,LowStockThreshold,CategoryId,Category,IsDeleted")]
        Product product)
    {
        if (!_productLogic.ValidateProductId(id, product))
        {
            return NotFound();
        }

        _productLogic.BindOrderItemsObjectToProduct(product);

        ModelState.ClearValidationState("Category");
        ModelState.ClearValidationState("OrdersItems");
        TryValidateModel(product);

        if (ModelState.IsValid)
        {
            await _productLogic.UpdateProductAsync(product);

            if (await _productLogic.GetProductByIdAsync(id) == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        return View(product);
    }

    /// <summary>
    /// Render confirmation page of the product deletion by ID
    /// </summary>
    /// <param name="id">ID of the product to delete</param>
    /// <returns></returns>
    [HttpGet("Delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productLogic.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


    /// <summary>
    /// Delete product by id
    /// </summary>
    /// <param name="ProductId"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int ProductId)
    {
        var product = await _productLogic.GetProductByIdAsync(ProductId);

        if (product == null)
        {
            return NotFound();
        }

        await _productLogic.DeleteProductAsync(product);

        return RedirectToAction("Index");
    }


    /// <summary>
    /// Render the product detail by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productLogic.GetProductWithCategoryByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet("Summary")]
    public async Task<IActionResult> Summary()
    {
        var products = await _productLogic.GetAllLowStockProductsListAsync();
        ViewBag.TotalNumberOfStock = await _productLogic.GetTotalNumberOfStockAsync();
        ViewBag.CategoryList = await _productLogic.GetAllCategoriesListAsync();
        return View(products);
    }
}