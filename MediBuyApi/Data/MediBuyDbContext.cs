using MediBuyApi.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediBuyApi.Data
{
    public class MediBuyDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public MediBuyDbContext(DbContextOptions<MediBuyDbContext> options) : base(options)
        {            

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a23e1561-9be1-4d64-a446-d4ffc6a8ae7d";
            var writerRoleId = "ad8c6183-b762-4b6d-a79e-7f9f587aa558";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                   Id = readerRoleId,
                   ConcurrencyStamp = readerRoleId,
                   Name = "Reader",
                   NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                   Id = writerRoleId,
                   ConcurrencyStamp = writerRoleId,
                   Name = "Writer",
                   NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
