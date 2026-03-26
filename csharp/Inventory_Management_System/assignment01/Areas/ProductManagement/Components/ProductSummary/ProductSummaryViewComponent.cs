using assignment01.Areas.ProductManagement.Logic;
using assignment01.Data;
using Microsoft.AspNetCore.Mvc;

namespace assignment01.Areas.ProductManagement.Components.ProductSummary;

public class ProductSummaryViewComponent: ViewComponent
{
    private readonly ProductLogic _productLogic;

    // dependency injection
    public ProductSummaryViewComponent(ProductLogic productLogic)
    {
        _productLogic = productLogic;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _productLogic.GetAllLowStockProductsListAsync();
        ViewBag.TotalNumberOfStock = await _productLogic.GetTotalNumberOfStockAsync();
        ViewBag.CategoryList = await _productLogic.GetAllCategoriesListAsync();
        
        return View("/Areas/ProductManagement/Views/Shared/Product/Components/Default.cshtml", products);
    }
}