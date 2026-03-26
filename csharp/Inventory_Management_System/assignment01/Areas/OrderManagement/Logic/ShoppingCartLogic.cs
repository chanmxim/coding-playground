using System.Data.Common;
using System.Diagnostics;
using assignment01.Areas.OrderManagement.Models;
using assignment01.Data;
using assignment01.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.OrderManagement.Logic;

public class ShoppingCartLogic : IDisposable
{
    private string ShoppingCartId { get; set; }

    private ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ErrorLog _errorLog;


    private const string CartSessionKey = "CartId";

    public ShoppingCartLogic(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
        ErrorLog errorLog)
    {
        _db = context;
        _httpContextAccessor = httpContextAccessor;
        _errorLog = errorLog;
    }

    public async Task<string> AddToCartAsync(int ProductId)
    {
        string errorMessage = string.Empty;
        try
        {
            var res = await VerifyProductIdAsync(ProductId);
            if (res.Item1)
            {
                ShoppingCartId = GetCartId();

                var cartItem = await _db.ShoppingCartItems
                    .Include(it => it.Product)
                    .SingleOrDefaultAsync(
                        c => c.CartId == ShoppingCartId
                             && c.ProductId == ProductId);
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists.                 
                    cartItem = new CartItem
                    {
                        CartItemId = Guid.NewGuid().ToString(),
                        ProductId = ProductId,
                        CartId = ShoppingCartId,
                        Product = _db.Products.FirstOrDefault(
                            p => p.ProductId == ProductId),
                        Quantity = 1,
                        DateCreated = DateTime.Now.ToUniversalTime(),
                        UserId = _httpContextAccessor.HttpContext.User.Identity.GetUserId(),
                    };

                    _db.ShoppingCartItems.Add(cartItem);
                    errorMessage = $"Item {cartItem.Product.ProductName} added to cart";
                }
                else
                {
                    // If the item does exist in the cart,                  
                    // then add one to the quantity.          
                    if (cartItem.Product != null)
                    {
                        if (cartItem.Product.Quantity >= cartItem.Quantity + 1)
                        {
                            cartItem.Quantity++;
                            errorMessage =
                                $"Item {cartItem.Product.ProductName} already in the cart. Quantity: {cartItem.Quantity}";
                        }
                        else
                        {
                            errorMessage =
                                $"No more items in the stock for {cartItem.Product.ProductName}. Current quantity in the cart: {cartItem.Quantity}";
                        }
                    }
                }

                await _db.SaveChangesAsync();

                return errorMessage;
            }

            // if product is not found return error msg
            return res.Item2;
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }

        return errorMessage;
    }

    public void Dispose()
    {
        if (_db != null)
        {
            _db.Dispose();
            _db = null;
        }
    }

    public string GetCartId()
    {
        if (_httpContextAccessor.HttpContext.Session.GetString(CartSessionKey) == null)
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.User.Identity.Name))
            {
                _httpContextAccessor.HttpContext.Session.SetString(CartSessionKey,
                    _httpContextAccessor.HttpContext.User.Identity.Name);
            }
            else
            {
                // Generate a new random GUID using System.Guid class.     
                Guid tempCartId = Guid.NewGuid();
                _httpContextAccessor.HttpContext.Session.SetString(CartSessionKey, tempCartId.ToString());
            }
        }

        return _httpContextAccessor.HttpContext.Session.GetString(CartSessionKey);
    }

    public async Task<ICollection<CartItem>> GetCartItemsAsync()
    {
        ShoppingCartId = GetCartId();
        try
        {
            return await _db.ShoppingCartItems
                .Include(p => p.Product)
                .Where(c => c.CartId == ShoppingCartId).ToListAsync();
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<(bool success, string message, int quantity, double total)> UpdateItemQuantityAsync(int ProductId, int quantity)
    {
        string errorMessage = "Item quantity updated successfully";
        ShoppingCartId = GetCartId();
        try
        {
            var currentItem = await GetCurrentItemAsync(ProductId);
            if (currentItem != null)
            {
                if (currentItem.Quantity + quantity > 0)
                {
                    if (currentItem.Product.Quantity < currentItem.Quantity + quantity)
                    {
                        errorMessage = $"No more items in the stock for {currentItem.Product.ProductName}.";

                        return (true, errorMessage, currentItem.Quantity, currentItem.CalculateTotal());
                    }

                    currentItem.Quantity += quantity;
                    _db.ShoppingCartItems.Update(currentItem);
                }
                else
                {
                    currentItem.Quantity += quantity;
                    _db.ShoppingCartItems.Remove(currentItem);
                    errorMessage = $"Item {currentItem.Product.ProductName} removed from the cart.";
                }

                await _db.SaveChangesAsync();

                return (true, errorMessage, currentItem.Quantity, currentItem.CalculateTotal());
            }

            errorMessage = $"Item {currentItem.Product.ProductName} does not exist in the cart.";
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }

        return (false, errorMessage, 0, 0);
    }

    public async Task RemoveFromCartAsync(int ProductId)
    {
        ShoppingCartId = GetCartId();
        try
        {
            var currentItem = await GetCurrentItemAsync(ProductId);
            if (currentItem != null)
            {
                _db.ShoppingCartItems.Remove(currentItem);
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task EmptyCartAsync()
    {
        ShoppingCartId = GetCartId();
        try
        {
            _db.ShoppingCartItems.RemoveRange(_db.ShoppingCartItems.Where(c => c.CartId == ShoppingCartId));
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task<CartItem?> GetCurrentItemAsync(int ProductId)
    {
        try
        {
            return await _db.ShoppingCartItems
                .Include(p => p.Product)
                .Where(p => p.ProductId == ProductId && p.CartId == ShoppingCartId)
                .FirstOrDefaultAsync(p => p.ProductId == ProductId);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<(bool, string)> VerifyProductIdAsync(int ProductId)
    {
        string errorMessage = string.Empty;
        bool returnValue = false;
        try
        {
            returnValue = await _db.Products.AnyAsync(p => p.ProductId == ProductId);
            if (!returnValue)
            {
                errorMessage = $"Product id {ProductId} does not exist";
            }
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }

        return (returnValue, errorMessage);
    }

    public async Task<double> GetGrandTotalAsync()
    {
        var items = await GetCartItemsAsync();
        double grandTotal = items.Sum(item => item.CalculateTotal());
        
        return grandTotal;
    }
}