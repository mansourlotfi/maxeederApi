using ecommerceApi.Entities;
using ecommerceApi.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Data
{
    public class StoreContext : IdentityDbContext<User, Role, int>
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<CeoOptimization> CeoOptimizations { get; set; }
        public DbSet<CustomUserRole> CustomUserRoles { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
               .HasOne(a => a.Address)
               .WithOne()
               .HasForeignKey<UserAddress>(a => a.Id)
               .OnDelete(DeleteBehavior.Cascade);


            //builder.Entity<Setting>().Property(p => p.Id).ValueGeneratedNever();



            builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }

}