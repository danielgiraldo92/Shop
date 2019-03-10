namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");

            // Add user
            var user = await this.userHelper.GetUserByEmailAsync("danielgiraldo92@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Daniel",
                    LastName = "Giraldo",
                    Email = "danielgiraldo92@gmail.com",
                    UserName = "danielgiraldo92@gmail.com",
                    PhoneNumber = "123456987"
                };

                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            // Add products
            if (!this.context.Products.Any())
            {
                this.AddProduct("Sombrero", user);
                this.AddProduct("Reloj", user);
                this.AddProduct("Gafas", user);
                await this.context.SaveChangesAsync();
            }

            // Add other user
            user = await this.userHelper.GetUserByEmailAsync("pepito@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Pedro",
                    LastName = "Perez",
                    Email = "pepito@gmail.com",
                    UserName = "pepito@gmail.com",
                    PhoneNumber = "555555"
                };

                var result = await this.userHelper.AddUserAsync(user, "654321");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(user, "Customer");
            }

            isInRole = await this.userHelper.IsUserInRoleAsync(user, "Customer");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Customer");
            }

            user = await this.userHelper.GetUserByEmailAsync("camilo@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Camilo",
                    LastName = "Giraldo",
                    Email = "camilo@gmail.com",
                    UserName = "camilo@gmail.com",
                    PhoneNumber = "555555"
                };

                var result = await this.userHelper.AddUserAsync(user, "654321");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(user, "Customer");
            }

            isInRole = await this.userHelper.IsUserInRoleAsync(user, "Customer");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Customer");
            }
        }

        private void AddProduct(string name, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user
            });
        }
    }
}
