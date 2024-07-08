using Blazored.LocalStorage;
using EMS.Blazor.Data;
using EMS.Blazor.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
namespace EMS.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddRadzenComponents();
            builder.Services.AddServerSideBlazor();

            
            // Add Blazored LocalStorage
            builder.Services.AddBlazoredLocalStorage();

            // Add HttpClient service
            builder.Services.AddScoped<HttpClient>();

            builder.Services.AddScoped<EquipmentService>();
            builder.Services.AddScoped<ModelService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<ManufacturerService>();
            builder.Services.AddScoped<StatusService>();
            builder.Services.AddScoped<EquipmentTransferService>();
            builder.Services.AddScoped<MaintenanceScheduleService>();
            builder.Services.AddScoped<UsageHistoryService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<InventoryOrderService>();
            builder.Services.AddScoped<OrderDetailService>();
            builder.Services.AddScoped<InventoryService>();
            builder.Services.AddScoped<ReplacementRecordService>();

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<RadzenDialog>();


            // Add service Blazorise
            builder.Services.AddBlazorise(options =>
            {
                options.Immediate = true;
            })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
