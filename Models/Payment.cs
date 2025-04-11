using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; } // Foreign Key to Order
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // Navigation property
    public virtual Order Order { get; set; }
}
}