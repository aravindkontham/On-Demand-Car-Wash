using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Models
{
    public class Leaderboard
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign Key to User
    public decimal WaterSavedGallons { get; set; }
    public DateTime LastUpdated { get; set; }

    // Navigation property
    public virtual User User{ get; set; }
}
}