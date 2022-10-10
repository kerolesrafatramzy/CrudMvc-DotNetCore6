using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Simple_blog.Models;

namespace Simple_blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                  builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
            {
                ProgressBar = true, 
                PositionClass = NToastNotify.ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseButton = true,
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}