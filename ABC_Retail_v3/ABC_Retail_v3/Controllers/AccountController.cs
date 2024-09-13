using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ABC_Retail_v3.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using WindowsAzure.Table.Extensions;
using Azure;
using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ABC_Retail_v3.Controllers
{
    public class AccountController : Controller
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ILogger<AccountController> _accountLogger;

        public AccountController(ILogger<AccountController> logger, TableServiceClient tableServiceClient)
        {
           
            _tableServiceClient = tableServiceClient;
            _accountLogger = logger;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task CreateTableAsync(string tableName)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CraftUsers model)
        {
            CreateTableAsync("CraftUsers");
            var tableClient = _tableServiceClient.GetTableClient("CraftUsers");

            model.PartitionKey = model.Email;

            if (ModelState.IsValid)
            {
                string hashedPassword = Crypto.HashPassword(model.Password);
                var craftUser = new CraftUsers()
                {
                    PartitionKey = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Password = hashedPassword 
                };


                try
                {
                    await tableClient.AddEntityAsync(craftUser);
                    _accountLogger.LogInformation("New User Registered");

                    return RedirectToAction("Our_Work", "Home");
                }
                catch (StorageException ex)
                {
                    Console.WriteLine($"Error code: {ex.RequestInformation.HttpStatusCode}");
                    Console.WriteLine($"Error message: {ex.Message}");
                    if (ex.RequestInformation.ExtendedErrorInformation != null)
                    {
                        Console.WriteLine($"Request URL: {ex.RequestInformation.ExtendedErrorInformation.ErrorMessage}");
                    }
                    ModelState.AddModelError("", "An error occurred while saving your profile. Please try again.");
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                CreateTableAsync("CraftUsers");
                var tableClient = _tableServiceClient.GetTableClient("CraftUsers");
                var users = await tableClient.GetEntityIfExistsAsync<CraftUsers>(model.Email, model.Email);
                try
                {
                    if (users != null)
                    {

                        
                        if (users.Value.Password== model.Password)
                        {
                            // Set session or authentication ticket (or use Identity if integrated)
                            //HttpContext.Session.SetString("CraftUsers", user.Value.RowKey);
                            _accountLogger.LogInformation("User Logged In");

                            return RedirectToAction("Our_Work", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid login attempt. Please check your password.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found. Please check your email.");
                    } 


                    ModelState.AddModelError("", "Invalid login attempt.");
                }
                catch (StorageException ex)
                {
                    Console.WriteLine($"Error code: {ex.RequestInformation.HttpStatusCode}");
                    Console.WriteLine($"Error message: {ex.Message}");
                    if (ex.RequestInformation.ExtendedErrorInformation != null)
                    {
                        Console.WriteLine($"Request URL: {ex.RequestInformation.ExtendedErrorInformation.ErrorMessage}");
                    }
                    ModelState.AddModelError("", "An error occurred while retrieving your profile. Please try again.");
                }
            }

            return View(model);
        }
    }
}



