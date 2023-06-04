using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.DAL
{
    public class RazorDbContext : IdentityDbContext<User>
    {
        public RazorDbContext(DbContextOptions<RazorDbContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<AccessoryImage> AccessoryImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AccessoryCategory> AccessoryCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Ram> Rams { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<ProductRamMemory> ProductRamMemories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<AccessoryColor> AccessoryColors { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryInformation> DeliveryInformations { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(s => s.Key).IsUnique();

            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Brand>().HasIndex(c => c.Name).IsUnique();

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        property.SetColumnType("decimal(6,2)");
                    }
                }
            }
        }
    }
}
