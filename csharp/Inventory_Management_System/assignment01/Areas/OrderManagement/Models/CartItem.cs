using System.ComponentModel.DataAnnotations;
using assignment01.Areas.Identity.Models;
using assignment01.Areas.ProductManagement.Models;

namespace assignment01.Areas.OrderManagement.Models;

public class CartItem
{
    [Key] public string CartItemId { get; set; }
    
    [Required]
    [Display(Name = "Cart ID")]
    public required string CartId { get; set; }
    
    [Range(0, int.MaxValue)] 
    public int Quantity { get; set; }
    
    [Display(Name = "Parent product ID")]
    public int ProductId { get; set; }
    [Display(Name = "Parent product")]
    public Product? Product { get; set; }
    
    [Display(Name = "Creation date")]
    public DateTime DateCreated { get; set; }
    
    [Required]
    [Display(Name = "User ID")]
    public required string UserId { get; set; }
    
    [Display(Name = "User")]
    public User? User { get; set; }
    
    
    public double CalculateTotal()
    {
        return Math.Round(Quantity * Product?.Price ?? 0, 2);
    }
}