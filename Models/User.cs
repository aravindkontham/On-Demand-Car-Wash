using System;
using Microsoft.AspNetCore.Identity;

namespace CarWash.Models
{
    public class User : IdentityUser<int> // Changed to IdentityUser<int> to use int as primary key
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } // One User to Many Orders (Customer)
        public virtual ICollection<Order> WasherOrders { get; set; } // One User to Many Orders (Washer)
        public virtual ICollection<Review> Reviews { get; set; } // One User to Many Reviews (Reviewer)
        public virtual ICollection<Review> Reviewed { get; set; } // One User to Many Reviews (Reviewee)
        public virtual ICollection<Report> Reports { get; set; } // One User to Many Reports
        public virtual ICollection<PromoCode> PromoCodes { get; set; } // One User to Many PromoCodes (if applicable)
        public virtual Leaderboard Leaderboard { get; set; } // One User to One Leaderboard entry
        public virtual ICollection<Log> Logs { get; set; } // One User to Many Logs

        public User()
        {
            Orders = new HashSet<Order>();
            WasherOrders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            Reviewed = new HashSet<Review>();
            Reports = new HashSet<Report>();
            PromoCodes = new HashSet<PromoCode>();
            Logs = new HashSet<Log>();
        }
    }
}