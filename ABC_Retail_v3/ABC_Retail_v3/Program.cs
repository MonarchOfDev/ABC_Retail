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
using ABC_Retail_v3.AzureStorageService.Interface;
using ABC_Retail_v3.AzureStorageService.Service;
using ABC_Retail_v3.AzureQueueService.Interface;
using ABC_Retail_v3.AzureQueueService.Service;
using ABC_Retail_v3.AzureTableService.Interface;
using ABC_Retail_v3.AzureTableService.Service;
using System.Configuration;
using ABC_Retail_v3.Models;
using Microsoft.AspNetCore.Identity;
using ABC_Retail_v3.Services;

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

            builder.Services.AddSingleton<ITableStorageService<CraftUsers>>(provider =>
            new TableStorageService<CraftUsers>(
                builder.Configuration.GetConnectionString("AzureTableStorage"),
                "CraftUsers"));

            builder.Services.AddSingleton<IBlobStorageService>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
                return new BlobStorageService(connectionString);
            });

            builder.Services.AddSingleton<IFileStorageService>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("AzureFilesStorage");
                return new FileStorageService(connectionString);
            });
            builder.Services.AddSingleton<IQueueStorageService>(provider =>
            new QueueStorageService(builder.Configuration.GetConnectionString("AzureQueueStorage"), "registeruser"));
            builder.Services.AddSingleton(typeof(ITableStorageService<>), typeof(TableStorageService<>));
            builder.Services.AddScoped<IPasswordHasher<CraftUsers>, PasswordHasher<CraftUsers>>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<AzureFunctionService>();
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
