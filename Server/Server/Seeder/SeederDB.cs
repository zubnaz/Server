using BuisnesLogic.Constants;
using Data.DBContext;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seeder
{
    public static class SeederDB
    {
        public static async Task SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<UserEntity>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<RoleEntity>>();

                #region Seed Roles and Users

                if (!context.Roles.Any())
                {
                    foreach (var role in Roles.All)
                    {
                        var result = roleManager.CreateAsync(new RoleEntity
                        {
                            Name = role
                        }).Result;
                    }
                }

                if (!context.Users.Any())
                {
                    UserEntity user = new()
                    {
                        FirstName = "Іван",
                        LastName = "Капот",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                    };
                    var result = userManager.CreateAsync(user, "123456")
                        .Result;
                    if (result.Succeeded)
                    {
                        result = userManager
                            .AddToRoleAsync(user, Roles.Admin)
                            .Result;
                    }
                }

                #endregion

                if (!context.Categories.Any())
                {
                    var kurtki = new Category
                    {
                        Name = "Куртка баті",
                        Description = "куртка баті",
                        Image = "kurtka.jpg",
                        UserId = 1
                        
                    };
                    var futbolki = new Category
                    {
                        Name = "Футболки",
                        Description = "Класна футболка",
                        Image = "futbolka.jpg",
                        UserId = 1

                    };
                    context.Categories.Add(kurtki);
                    context.Categories.Add(futbolki);
                    context.SaveChanges();
                }
            }
        }
    }
}
