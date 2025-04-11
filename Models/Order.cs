using System;
using System.Collections.Generic;

namespace CarWash.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Foreign Key to User (Customer), changed to string
        public int? WasherId { get; set; }  // Foreign Key to User (Washer), changed to string, nullable
        public int CardId { get; set; }        // Foreign Key to Payment (assuming Payment.Id is int)
        public int PackageId { get; set; }     // Foreign Key to Package (matches Package.Id as int)
        public int? PromoCodeId { get; set; }  // Foreign Key to PromoCode (matches PromoCode.Id as int), nullable
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public virtual User Customer { get; set; }
        public virtual User? Washer { get; set; } // Nullable to match WasherId
        public virtual Payment Payment { get; set; }
        public virtual Package Package { get; set; }
        public virtual PromoCode? PromoCode { get; set; } // Nullable to match PromoCodeId
        public virtual ICollection<OrderAddOn> OrderAddOns { get; set; }
        public virtual ICollection<Report> Reports { get; set; }

        public Order()
        {
            OrderAddOns = new HashSet<OrderAddOn>();
            Reports = new HashSet<Report>();
        }
    }
}