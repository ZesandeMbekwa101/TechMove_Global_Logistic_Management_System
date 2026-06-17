using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Data;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Helpers;
using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SearchFilterHelper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileValidationService>();
builder.Services.AddScoped<ContractBusinessService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();

// JWT AUTH
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// REGISTER SERVICES (IMPORTANT)
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuditLogHelper>();
builder.Services.AddHttpClient<FastForexService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<FastForexService>();


var app = builder.Build();

app.UseHttpsRedirection();

// 🔥 THIS WAS MISSING
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Scalar UI
}

app.MapControllers();
app.UseStaticFiles();

app.Run();
public partial class Program
{
}







