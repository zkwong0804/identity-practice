using IdentityPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<FoodDbContext>(opt => {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("db"));
            });

            builder.Services.AddIdentity<FoodUser, FoodRole>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<FoodDbContext>();

            builder.Services.ConfigureApplicationCookie(opt => {
                opt.LoginPath = "/account/login";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}