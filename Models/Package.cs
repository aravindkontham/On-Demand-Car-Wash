using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
   public class Package
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
   
    public bool IsActive { get; set; }

    // Navigation property
    public virtual ICollection<Order> Orders { get; set; } // One Package to Many Orders

    public Package()
    {
        Orders = new HashSet<Order>();
    }
}
}