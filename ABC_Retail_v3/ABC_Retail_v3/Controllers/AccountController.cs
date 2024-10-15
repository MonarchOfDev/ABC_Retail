using Microsoft.AspNetCore.Mvc;
using ABC_Retail_v3.Models;
using Azure.Data.Tables;
using ABC_Retail_v3.AzureTableService.Interface;
using Microsoft.AspNetCore.Identity;
using ABC_Retail_v3.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ABC_Retail_v3.Controllers
{
    public class AccountController : Controller
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ITableStorageService<CraftUsers> _tableStorageService;
        private readonly ILogger<AccountController> _accountLogger;
        private readonly IPasswordHasher<CraftUsers> _passwordHasher;
        private readonly AzureFunctionService _azureFunctionService;

        public AccountController(ILogger<AccountController> logger
            , TableServiceClient tableServiceClient
            , ITableStorageService<CraftUsers> tableStorageService,
            IPasswordHasher<CraftUsers> passwordHasher,
            AzureFunctionService azureFunctionService)
        {


            _azureFunctionService = azureFunctionService;
            _tableServiceClient = tableServiceClient;
            _tableStorageService = tableStorageService;
            _accountLogger = logger;
            _passwordHasher = passwordHasher;

        }

        [HttpGet]
        public IActionResult Register()
        {
            //var model = new CraftUsers
            //{
            //    RoleList = GetRoleList()
            //};
            //return View(model);
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
            if (ModelState.IsValid)
            {
                try
                {
                    // Validate user data
                    model.ValidateUser();

                    // Hash the password using ASP.NET Core's PasswordHasher
                    model.Password = _passwordHasher.HashPassword(model, model.Password);
                    model.ConfirmPassword = null; // Clear confirm password

                    // Call Azure Function to store user in Table Storage
                    var storeUserResult = await _azureFunctionService.StoreUserAsync(model);
                    _accountLogger.LogInformation($"User {model.Email} stored via Azure Function.");

                    // Optionally, enqueue a transaction message
                    var transaction = new
                    {
                        Email = model.Email,
                        Action = "Register",
                        Timestamp = DateTime.UtcNow
                    };
                    var enqueueResult = await _azureFunctionService.AddToQueueAsync(transaction);
                    _accountLogger.LogInformation($"Transaction enqueued for user {model.Email}.");

                    return RedirectToAction("Our_Work", "Home");
                }
                catch (HttpRequestException ex)
                {
                    _accountLogger.LogError($"HTTP Request error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while registering. Please try again.");
                }
                catch (Exception ex)
                {
                    _accountLogger.LogError($"Exception: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
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
                try
                {
                    // Call Azure Function to retrieve user data
                    var user = await _azureFunctionService.GetUserAsync(model.Email, model.Email);

                    if (user != null)
                    {
                        // Verify the password
                        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                        if (verificationResult == PasswordVerificationResult.Success)
                        {
                            // Implement authentication logic (e.g., set cookies, JWT tokens)
                            _accountLogger.LogInformation($"User {model.Email} logged in successfully.");

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
                }
                catch (HttpRequestException ex)
                {
                    _accountLogger.LogError($"HTTP Request error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while logging in. Please try again.");
                }
                catch (Exception ex)
                {
                    _accountLogger.LogError($"Exception: {ex.Message}");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(model);
        }
    }
}



