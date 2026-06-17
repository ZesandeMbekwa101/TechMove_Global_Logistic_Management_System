using Microsoft.EntityFrameworkCore;
using TechMove_Global_Logistic_Management_System.Services;

namespace TechMove_Global_Logistic_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<AuditLogService>();
            builder.Services.AddScoped<ContractService>();
            builder.Services.AddHttpClient<ServiceRequestService>();
            builder.Services.AddScoped<ServiceRequestService>();
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            builder.Services.AddHttpClient("TechMoveAPI", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ApiSettings:BaseUrl"]);
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
