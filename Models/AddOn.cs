using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class AddOn
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // Navigation property
    public virtual ICollection<OrderAddOn> OrderAddOns { get; set; } // One AddOn to Many OrderAddOns

    public AddOn()
    {
        OrderAddOns = new HashSet<OrderAddOn>();
    }
}
}