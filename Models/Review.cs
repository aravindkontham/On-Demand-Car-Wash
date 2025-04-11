using System;

namespace CarWash.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; } // Foreign Key to User (Reviewer), changed to string
        public int ReviewedId { get; set; } // Foreign Key to User (Reviewed), changed to string
        public int OrderId { get; set; }       // Foreign Key to Order (remains int)
        public string Comment { get; set; }    // Example property
        public int Rating { get; set; }        // Example property

        // Navigation properties
        public virtual User Reviewer { get; set; }
        public virtual User Reviewed { get; set; }
        public virtual Order Order { get; set; }
    }
}