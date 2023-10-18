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
        public DbSet<Feature> Features { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SeoOptimization> SeoOptimizations { get; set; }
        public DbSet<CustomUserRole> CustomUserRoles { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<MainMenu> MainMenuItems { get; set; }
        public DbSet<QuickAccess> QuickAccess { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<PageItem> PageItems { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DbSet<Size> Sizes { get; set; }









        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
               .HasOne(a => a.Address)
               .WithOne()
               .HasForeignKey<UserAddress>(a => a.Id)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductFeature>()
                .HasKey(pf => new { pf.ProductId, pf.FeatureId });
            builder.Entity<ProductFeature>()
                .HasOne(pf => pf.Product)
                .WithMany(pf=>pf.Features)
                .HasForeignKey(pf=>pf.ProductId);
            builder.Entity<ProductFeature>()
                .HasOne(pf => pf.Feature)
                .WithMany(pf => pf.Products)
                .HasForeignKey(pf => pf.FeatureId);

            builder.Entity<SocialNetwork>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<MainMenu>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<QuickAccess>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<Logo>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<Category>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<Slide>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<Article>().HasIndex(u => u.Priority).IsUnique();
            builder.Entity<SeoOptimization>().HasIndex(u => u.Priority).IsUnique();



            //builder.Entity<Setting>().Property(p => p.Id).ValueGeneratedNever();

            builder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }

}