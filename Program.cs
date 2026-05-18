using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Data;
using TechMove_Global_Logistic_Management_System.Helpers;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();
            builder.Services.AddScoped<CurrencyService>();
            builder.Services.AddScoped<FileValidationService>();
            builder.Services.AddScoped<ContractBusinessService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<SearchFilterHelper>();
            builder.Services.AddScoped<AuditLogHelper>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=DashboardView}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
