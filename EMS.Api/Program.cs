
using EMS.Api.Controllers;
using EMS.Data;
using EMS.Services;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace EMS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<MyDbContext>(options => 
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            /*builder.Services.AddScoped<ISerialNumberService, SerialNumberService>(); */// Dang ky dich vu
            builder.Services.AddScoped<IModelService, ModelService>();
            builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IStatusService, StatusService>();
            builder.Services.AddScoped<IEquipmentTransferService, EquipmentTransferService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUsageHistoryService, UsageHistoryService>();
            builder.Services.AddScoped<IMaintenanceScheduleService, MaintenanceScheduleService>();
            builder.Services.AddScoped<IInventoryOrderService, InventoryOrderService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IReplacementRecordService, ReplacementRecordService>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<TokenService>();
            //JWT
            var secretKey = builder.Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role, // Xa định claim cho role
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
            });
            //Cau hinh  JSON serialization với ReferenceHandler.Preserve:
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var tokenService = context.RequestServices.GetRequiredService<TokenService>();

                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (context.Request.Path.StartsWithSegments("/api/Users/Login") || context.Request.Path.StartsWithSegments("/api/Users/Logout"))
                {
                    await next();
                    return;
                }

                // Kiểm tra token cho một số phương thức
                if (context.Request.Method == HttpMethods.Post ||
                    context.Request.Method == HttpMethods.Put ||
                    context.Request.Method == HttpMethods.Delete ||
                    (context.Request.Method == HttpMethods.Get &&
                    context.Request.Path.StartsWithSegments("/api/Users")))
                {
                    if (token != null)
                    {
                        if (await tokenService.IsTokenRevoked(token))
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Token has been revoked");
                            return;
                        }
                    }

                    else
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Token is missing");
                        return;
                    }
                }

                await next();
            });
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
