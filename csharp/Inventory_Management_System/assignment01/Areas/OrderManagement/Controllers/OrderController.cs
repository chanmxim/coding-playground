using assignment01.Areas.OrderManagement.Logic;
using assignment01.Controllers;
using assignment01.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.OrderManagement.Controllers;

[Route("Order")]
[Area("OrderManagement")]
[Authorize]
public class OrderController : BaseController
{
    private readonly ApplicationDbContext _context;
    private ShoppingCartLogic _shoppingCartLogic;
    private OrderLogic _orderLogic;

    public OrderController(ShoppingCartLogic shoppingCartLogic, ApplicationDbContext context, OrderLogic orderLogic)
    {
        _shoppingCartLogic = shoppingCartLogic;
        _context = context;
        _orderLogic = orderLogic;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .ToListAsync();
        return View(orders);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        string errorMsg = string.Empty;
        var cartItems = await _shoppingCartLogic.GetCartItemsAsync();
        var res = await _orderLogic.CreateOrderAsync(cartItems);
        if (res.Item1)
        {
            await _shoppingCartLogic.EmptyCartAsync();
            TempData["Message"] = res.Item2;
            return RedirectToAction("Index");
        }

        TempData["Message"] = errorMsg;
        
        return RedirectToAction("Index", "CartItem");
    }

    [HttpGet("Details")]
    public async Task<IActionResult> Details(int orderId)
    {
        var order = await _orderLogic.GetOrderDetailsAsync(orderId);

        if (order == null) return NotFound();

        return View(order);
    }
}