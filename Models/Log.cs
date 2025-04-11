using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class Log
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
    public int? UserId { get; set; } // Foreign Key to User

    // Navigation property
    public virtual User User { get; set; }
}
}