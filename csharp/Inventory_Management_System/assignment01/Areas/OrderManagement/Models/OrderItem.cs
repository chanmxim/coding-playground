using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using assignment01.Areas.ProductManagement.Models;
using assignment01.Models;

namespace assignment01.Areas.OrderManagement.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int OrderItemQuantity { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public double OrderItemPrice { get; set; }

    public double CalculateTotal()
    {
        return OrderItemPrice * OrderItemQuantity;
    }
}