using ecommerceApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace ecommerceApi.Data
{
    public static class DbInitializer
    {
        public static async Task Inilialize(StoreContext context, UserManager<User> userManager)
        {

            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "user",
                    Email = "user@test.com",
                    PhoneNumber="091222"

                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@maxeeder.com"
                };

            
                    await userManager.CreateAsync(admin, "Pa$$w0rd!!@@");
                    await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
            
                
            }

            if (context.Categories.Any()) return;
            var categories = new List<Category>()
            {
                new Category
                {
                    Id = 1,
                    Name="دسته بندی 1",
                    PictureUrl="638231191664402793.jpg"
                },
                 new Category
                {
                    Id = 2,
                    Name="دسته بندی21",
                    PictureUrl="638231191664402793.jpg"
                }

            };

            foreach (var item in categories)
            {
                context.Categories.Add(item);
            }
            context.SaveChanges();

            if (context.Brands.Any()) return;
            var brands = new List<Brand>()
            {
                new Brand
                {
                    Id = 1,
                    Name="برند 1",
                    PictureUrl="638231191664402793.jpg"

                },
                 new Brand
                {
                    Id = 2,
                    Name="برند 2",
                    PictureUrl="638231191664402793.jpg"

                }

            };

            foreach (var item in brands)
            {
                context.Brands.Add(item);
            }
            context.SaveChanges();


            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "محصول 1",
                    Description =
                        "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است",
                    Price = 20000,
                    PictureUrl = "638231191664402793.jpg",
                    Brand = "برند 1",
                    Type = "دسته بندی 1",
                    QuantityInStock = 100,
                    IsFeatured=false,
                    
                    
                },
                new Product
             {
                    Name = "محصول 2",
                    Description =
                        "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است",
                    Price = 20000,
                    PictureUrl = "638231191664402793.jpg",
                    Brand = "برند 2",
                    Type = "دسته بندی 2",
                    QuantityInStock = 100,
                    IsFeatured=false
                },


};

            foreach (var item in products)
            {
                context.Products.Add(item);
            }
            context.SaveChanges();


        }

    }
}