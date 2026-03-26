using System.ComponentModel.DataAnnotations;

namespace assignment01.Areas.ProductManagement.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Display(Name = "Category name")]
    [Required(ErrorMessage = "Category is required")]
    [StringLength(255, ErrorMessage = "Category name cannot be longer than 255 characters")]
    public string CategoryName { get; set; }
    
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    public string? CategoryDescription { get; set; }
    
    
    [Display(Name = "Child collection of products")]
    public ICollection<Product>? Products { get; set; }
}