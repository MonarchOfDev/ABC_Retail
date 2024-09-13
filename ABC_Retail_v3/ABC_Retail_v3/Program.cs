using ABC_Retail_v3.AzureBlobService.Interface;
using ABC_Retail_v3.AzureBlobService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Azure;
using Azure.Storage.Files;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Azure.Data.Tables;

namespace ABC_Retail_v3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddSingleton(x =>
            new TableServiceClient(builder.Configuration.GetConnectionString("AzureTableStorage")));

            builder.Services.AddSingleton<IBlobStorageService>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("BlobStorage");
                return new BlobStorageService(connectionString);
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Logging.AddAzureWebAppDiagnostics();
            builder.Services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "logs-";
                options.FileSizeLimit = 50 + 1024;
                options.RetainedFileCountLimit = 5;
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
