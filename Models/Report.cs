using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class Report
{
    public int Id { get; set; }
    public int OrderId { get; set; } // Foreign Key to Order
    public string WasherName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }

    // Navigation property
    public virtual Order Order { get; set; }
}
}