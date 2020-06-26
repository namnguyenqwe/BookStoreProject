using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStoreProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStoreProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = CreateHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetRequiredService<BookStoreDbContext>();
                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                dbContext.Database.Migrate();

                if (!dbContext.Roles.Any(r => r.Name == "Customer manager" || r.Name == "Book manager"))
                {
                    #region Add permission for admin
                    var adminRole = roleMgr.FindByNameAsync("Admin").GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "BOOK")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "CATEGORY")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "PUBLISHER")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "USER")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "COUPON")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "REVIEW")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "SHIPPING")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "SUBSCRIBER")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(adminRole, new Claim("PERMISSION", "ORDER")).GetAwaiter().GetResult();
                    #endregion

                    #region Customer manager role and permission
                    var customerManagerRole = new IdentityRole();
                    customerManagerRole.Name = "Customer manager";
                    roleMgr.CreateAsync(customerManagerRole).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "USER")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "COUPON")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "REVIEW")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "SHIPPING")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "SUBSCRIBER")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(customerManagerRole, new Claim("PERMISSION", "ORDER")).GetAwaiter().GetResult();
                    #endregion

                    #region Book manager role and permission
                    var bookManagerRole = new IdentityRole();
                    bookManagerRole.Name = "Book manager";
                    roleMgr.CreateAsync(bookManagerRole).GetAwaiter().GetResult(); ;
                    roleMgr.AddClaimAsync(bookManagerRole, new Claim("PERMISSION", "BOOK")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(bookManagerRole, new Claim("PERMISSION", "CATEGORY")).GetAwaiter().GetResult();
                    roleMgr.AddClaimAsync(bookManagerRole, new Claim("PERMISSION", "PUBLISHER")).GetAwaiter().GetResult();
                    #endregion
                }
          
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
