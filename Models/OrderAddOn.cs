using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class OrderAddOn
{
    public int OrderId { get; set; } // Foreign Key to Order
    public int AddOnId { get; set; } // Foreign Key to AddOn

    // Navigation properties
    public virtual Order Order { get; set; }
    public virtual AddOn AddOn { get; set; }
}
}