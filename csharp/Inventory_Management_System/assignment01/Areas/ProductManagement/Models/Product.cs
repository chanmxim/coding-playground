using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using assignment01.Areas.OrderManagement.Models;
using assignment01.Models;

namespace assignment01.Areas.ProductManagement.Models;

public class Product
{
    /// <summary>
    /// Primary key for Products
    /// </summary>
    [Key]
    public int ProductId { get; set; }
    /// <summary>
    /// Name of the project with maximum length of 255
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters")]
    [Display(Name = "Product name")]
    public string ProductName {get; set;}
    
    [Display(Name = "Product description")]
    [DataType(DataType.MultilineText)]
    public string? ProductDescription {get; set;}
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency,ErrorMessage = "Price cannot be negative")]
    [Display(Name = "Price")]
    public double Price {get; set;}
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = $"Quantity cannot be negative or grater than 2,147,483,647")]
    [Display(Name = "Quantity")]
    public int Quantity {get; set;}
    
    [Required(ErrorMessage = "Low stock threshold is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Low stock threshold cannot be negative")]
    [Display(Name = "Low stock threshold")]
    public int LowStockThreshold {get; set;}
    
    /// <summary>
    /// Foreign key for Category
    /// </summary>
    [Display(Name = "Parent category ID")]
    public int CategoryId {get; set;}
    /// <summary>
    /// Represents a Category itself
    /// </summary>
    [Display(Name = "Parent category")]
    public Category? Category {get; set;}
    

    /// <summary>
    /// Represents m:m relationship between Product and Order models
    /// </summary>
    [Display(Name = "Child collection of products in the order")]
    public ICollection<OrderItem>? OrdersItems {get; set;}
    
    [Required]
    [Display(Name = "Is Soft Deleted")]
    public bool IsDeleted {get; set;} = false;
    
    [Display(Name = "Child collection of products in the cart")]
    public ICollection<CartItem>? CartItems {get; set;}
}