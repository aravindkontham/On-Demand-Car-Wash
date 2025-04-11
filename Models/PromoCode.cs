using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation property
    public virtual ICollection<Order> Orders { get; set; } // One PromoCode to Many Orders

    public PromoCode()
    {
        Orders = new HashSet<Order>();
    }
}
}