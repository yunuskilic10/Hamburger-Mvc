using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCProjectHamburger.Data;
using MVCProjectHamburger.Models.Entities.Concrete;
using MVCProjectHamburger.Models.Utilities;
using System;

namespace MVCProjectHamburger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("ConStr");
            builder.Services.AddDbContext<HamburgerDbContext>(options =>
                options.UseSqlServer(connectionString));

                //        builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //.AddEntityFrameworkStores<HamburgerDbContext>();

            builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<HamburgerDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Panel}/{action=Index}/{id?}"
                );
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();


            //var scope = app.Services.CreateScope();
            //var userManager = (UserManager<AppUser>)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>));
            //ForLogin.AddSuperUserAsync(userManager);

            app.Run();
        }
    }
}