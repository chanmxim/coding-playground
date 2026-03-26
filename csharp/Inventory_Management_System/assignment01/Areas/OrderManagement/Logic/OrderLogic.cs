using assignment01.Areas.Identity.Models;
using assignment01.Areas.OrderManagement.Models;
using assignment01.Data;
using assignment01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.OrderManagement.Logic;

public class OrderLogic
{
    private ApplicationDbContext _db;
    private UserManager<User> _userManager;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly ErrorLog _errorLog;

    public OrderLogic(ApplicationDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
        ErrorLog errorLog)
    {
        _db = db;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _errorLog = errorLog;
    }

    public async Task<(bool, string)> CreateOrderAsync(ICollection<CartItem> cartItems)
    {
        string errorMsg = string.Empty;
        try
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                errorMsg = "User not logged in";
                return (false, errorMsg);
            }

            if (cartItems == null || cartItems.Count == 0)
            {
                errorMsg = string.Empty;
                return (false, errorMsg);
            }

            if (VerifyCart(cartItems, out string errorMessage))
            {
                var order = new Order
                {
                    UserId = user.Id,
                    DeliveryAddress = user.Address
                };
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                ICollection<OrderItem> orderItems = new List<OrderItem>();
                foreach (var item in cartItems)
                {
                    orderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        OrderItemQuantity = item.Quantity,
                        OrderItemPrice = item.Product.Price
                    });
                    var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                    }
                }

                _db.OrderItems.AddRange(orderItems);
                await _db.SaveChangesAsync();

                errorMsg = string.Empty;
                return (true, errorMsg);
            }

            errorMsg = errorMessage;
            return (false, errorMsg);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    private bool VerifyCart(ICollection<CartItem> cartItems, out string errorMsg)
    {
        var returnValue = true;
        errorMsg = string.Empty;
        foreach (var item in cartItems)
        {
            if (item.Quantity > item.Product.Quantity)
            {
                errorMsg +=
                    $"Quantity {item.Quantity} for {item.Product.ProductName} is greater than the product quantity {item.Product.Quantity}\n";
                returnValue = false;
            }
        }

        return returnValue;
    }

    public async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        try
        {
            return await _db.Orders
                .Include(oi => oi.OrderItems)
                .ThenInclude(po => po.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }
}