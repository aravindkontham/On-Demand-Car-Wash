using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarWash.Models;

namespace CarWash.Data
{
    public class GreenWashDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<OrderAddOn> OrderAddOns { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Log> Logs { get; set; }

        public GreenWashDbContext(DbContextOptions<GreenWashDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Relationships
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.WasherOrders)
                .WithOne(o => o.Washer)
                .HasForeignKey(o => o.WasherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Reviewer)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviewed)
                .WithOne(r => r.Reviewed)
                .HasForeignKey(r => r.ReviewedId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.PromoCodes)
                .WithMany();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Leaderboard)
                .WithOne(l => l.User)
                .HasForeignKey<Leaderboard>(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Logs)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Car>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Package>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Package)
                .HasForeignKey(o => o.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<PromoCode>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.PromoCode)
                .HasForeignKey(o => o.PromoCodeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderAddOns)
                .WithOne(oa => oa.Order)
                .HasForeignKey(oa => oa.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Reports)
                .WithOne(r => r.Order)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<OrderAddOn>()
                .HasKey(oa => new { oa.OrderId, oa.AddOnId });

            modelBuilder.Entity<OrderAddOn>()
                .HasOne(oa => oa.Order)
                .WithMany(o => o.OrderAddOns)
                .HasForeignKey(oa => oa.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderAddOn>()
                .HasOne(oa => oa.AddOn)
                .WithMany(a => a.OrderAddOns)
                .HasForeignKey(oa => oa.AddOnId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewed)
                .WithMany(u => u.Reviewed)
                .HasForeignKey(r => r.ReviewedId)
                .OnDelete(DeleteBehavior.NoAction); 

            
            modelBuilder.Entity<Leaderboard>()
                .HasOne(l => l.User)
                .WithOne(u => u.Leaderboard)
                .HasForeignKey<Leaderboard>(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<Log>()
            .HasOne(l => l.User)
            .WithMany(u => u.Logs)
            .HasForeignKey(l => l.UserId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Decimal Precision Configuration
            modelBuilder.Entity<AddOn>()
                .Property(a => a.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Leaderboard>()
                .Property(l => l.WaterSavedGallons)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Discount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Package>()
                .Property(p => p.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<PromoCode>()
                .Property(p => p.Discount)
                .HasPrecision(8, 2);
            #endregion

            #region Seed Data
            modelBuilder.Entity<Package>().HasData(
                new Package { Id = 1, Code = "STD", Description = "Standard Wash", Discount = 0m, Price = 20.00m, IsActive = true },
                new Package { Id = 2, Code = "PRE", Description = "Premium Wash", Discount = 5.00m, Price = 35.00m, IsActive = true },
                new Package { Id = 3, Code = "DEL", Description = "Deluxe Wash", Discount = 10.00m, Price = 50.00m, IsActive = true },
                new Package { Id = 4, Code = "ULT", Description = "Ultimate Wash", Discount = 15.00m, Price = 70.00m, IsActive = true },
                new Package { Id = 5, Code = "ECO", Description = "Eco-Friendly Wash", Discount = 0m, Price = 25.00m, IsActive = true }
            );

            var startDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<PromoCode>().HasData(
                new PromoCode { Id = 1, Code = "SAVE10", Description = "10% Off First Wash", Discount = 10.00m, IsPercentage = true, StartDate = startDate, EndDate = startDate.AddMonths(6), IsActive = true },
                new PromoCode { Id = 2, Code = "WELCOME5", Description = "5 USD Off", Discount = 5.00m, IsPercentage = false, StartDate = startDate, EndDate = startDate.AddMonths(6), IsActive = true },
                new PromoCode { Id = 3, Code = "SUMMER20", Description = "20% Off Summer Special", Discount = 20.00m, IsPercentage = true, StartDate = startDate, EndDate = startDate.AddMonths(3), IsActive = true },
                new PromoCode { Id = 4, Code = "NEWUSER", Description = "10 USD Off for New Users", Discount = 10.00m, IsPercentage = false, StartDate = startDate, EndDate = startDate.AddMonths(6), IsActive = true },
                new PromoCode { Id = 5, Code = "GREEN15", Description = "15% Off Eco Wash", Discount = 15.00m, IsPercentage = true, StartDate = startDate, EndDate = startDate.AddMonths(6), IsActive = true }
            );

            modelBuilder.Entity<AddOn>().HasData(
                new AddOn { Id = 1, Name = "Waxing", Description = "Car waxing service", Price = 15.00m },
                new AddOn { Id = 2, Name = "Interior Cleaning", Description = "Deep interior cleaning", Price = 20.00m },
                new AddOn { Id = 3, Name = "Tire Shine", Description = "Tire polishing", Price = 10.00m },
                new AddOn { Id = 4, Name = "Window Cleaning", Description = "Exterior window wash", Price = 12.00m },
                new AddOn { Id = 5, Name = "Odor Removal", Description = "Interior odor elimination", Price = 18.00m }
            );
            #endregion
        }
    }
}