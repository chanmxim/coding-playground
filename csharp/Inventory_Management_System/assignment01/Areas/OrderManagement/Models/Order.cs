using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using assignment01.Models;
using assignment01.Areas.Identity.Models;

namespace assignment01.Areas.OrderManagement.Models;

public class Order
{
    [Key] public int OrderId { get; set; }
    [ForeignKey("UserId")] 
    public string UserId { get; set; }
    public User User { get; set; }
    [Required] public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime DeliveryDate { get; set; }
    [Required] public string DeliveryAddress { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }

    // public ICollection<OrderStatus>? OrderStatuses { get; set; }


    public double CalculateGrandTotal()
    {
        double grandTotal = 0;
        if (OrderItems != null)
        {
            foreach (var orderItem in OrderItems)
            {
                grandTotal += orderItem.CalculateTotal();
            }
        }

        return Math.Round(grandTotal, 2);
    }
}