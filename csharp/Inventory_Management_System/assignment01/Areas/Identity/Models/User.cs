using System.ComponentModel.DataAnnotations;
using assignment01.Areas.OrderManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace assignment01.Areas.Identity.Models;

public class User: IdentityUser
{
    [StringLength(50,ErrorMessage = "First name cannot be longer than 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [StringLength(50,ErrorMessage = "Last name cannot be longer than 50 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(250,ErrorMessage = "Address cannot be longer than 250 characters.")]
    [Display(Name = "Address")]
    public string Address { get; set; }
    
    
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Child collection of orders")]
    public ICollection<Order>? Orders { get; set; }
    
    [Display(Name = "Child collection of cart items")]
    public ICollection<CartItem>? CartItems { get; set; }
    

}