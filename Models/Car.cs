using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class Car
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public string LicensePlate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    
    public virtual User User { get; set; }
}
}