using assignment01.Areas.OrderManagement.Logic;
using assignment01.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace assignment01.Areas.OrderManagement.Controllers;

[Route("Cart")]
[Area("OrderManagement")]
[Authorize]
public class CartItemController : BaseController
{
    private ShoppingCartLogic _shoppingCartLogic;
    private readonly ILogger<CartItemController> _logger;

    public CartItemController(ShoppingCartLogic shoppingCartLogic, ILogger<CartItemController> logger)
    {
        _shoppingCartLogic = shoppingCartLogic;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var cartItems = await _shoppingCartLogic.GetCartItemsAsync();
        return View(cartItems);
    }

    [HttpGet("AddItemToCart/{ProductId}")]
    public async Task<IActionResult> AddItemToCart(int ProductId)
    {
        string errorMsg = string.Empty;
        var res = await _shoppingCartLogic.VerifyProductIdAsync(ProductId);
        if (res.Item1)
        {
            errorMsg = await _shoppingCartLogic.AddToCartAsync(ProductId);
        }

        TempData["Message"] = errorMsg;
        return RedirectToAction("Index", "Product", new { area = "ProductManagement" });
    }

    [HttpPost("UpdateItemQuantity")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateItemQuantity([FromBody] Dictionary<string, string> formData)
    {
        string errorMsg = string.Empty;
        var quantityChange = 0;
        
        int productId = Convert.ToInt32(formData["ProductId"]);
        string action = formData["Action"];
        
        if (action == "increment")
        {
            quantityChange = 1;
        }
        else if (action == "decrement")
        {
            quantityChange = -1;
        }

        if (quantityChange != 0)
        {
            var res = await _shoppingCartLogic.UpdateItemQuantityAsync(productId, quantityChange);
            double grandTotal = await _shoppingCartLogic.GetGrandTotalAsync();

            return Json(new { success = res.success, message = res.message, quantity = res.quantity, total = res.total, grandTotal = grandTotal});
        }
        
        return Json(new { success = false, message = "Invalid \"Action\" parameter"});
    }

    [HttpPost("RemoveItemFromCart")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItemFromCart([FromBody] Dictionary<string, string> formData)
    {
        int productId = Convert.ToInt32(formData["ProductId"]);
        
        await _shoppingCartLogic.RemoveFromCartAsync(productId);
        double grandTotal = await _shoppingCartLogic.GetGrandTotalAsync();
        
        return Json(new { success = true, message = "Item Removed", grandTotal = grandTotal});
    }

    [HttpPost("ClearCart")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearCart()
    {
        await _shoppingCartLogic.EmptyCartAsync();
        return RedirectToAction("Index", "Product");
    }
}