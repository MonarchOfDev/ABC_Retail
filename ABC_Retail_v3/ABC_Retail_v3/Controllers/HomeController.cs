using ABC_Retail_v3.Models;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;
using Azure.Storage.Blobs;
using System.IO;
using ABC_Retail_v3.AzureBlobService.Service;
using ABC_Retail_v3.AzureBlobService.Interface;



namespace ABC_Retail_v3.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ILogger<HomeController> _accountLogger;
        private readonly IBlobStorageService _blobStorageService;

        public HomeController(ILogger<HomeController> logger, TableServiceClient tableServiceClient, IBlobStorageService blobStorageService)
        {
            _tableServiceClient = tableServiceClient;
            _accountLogger = logger;
            _blobStorageService = blobStorageService;
        }
        public IActionResult About_Us()
        {
            return View();
        }
        public IActionResult Contact_Us()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UploadProductImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductImage(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var fileName = file.FileName;
                        var blobUrl = await _blobStorageService.UploadFileAsync(fileName, stream);
                        ViewData["BlobUrl"] = blobUrl;
                    }
                }
                else
                {
                    // Handle the case where no file was uploaded
                    ViewData["Error"] = "No file selected for upload.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show an error message to the user
                Console.WriteLine($"Error in UploadProductImage: {ex.Message}");
                ViewData["Error"] = "An error occurred while uploading the file. Please try again.";
            }

            return View();
        }
        public async Task<IActionResult> Our_Work(string search)
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            var craftProducts = new List<Products>();

            try
            {
                // Fetch all users
                await foreach (Products user in tableClient.QueryAsync<Products>())
                {
                    craftProducts.Add(user);
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return StatusCode(500, "Error retrieving users.");
            }

            return View(craftProducts.ToList());

        }

        //        [HttpPost]
        //        public async Task<IActionResult> Our_Work(int id, Cart cart)
        //        {
        //            var Carts = await _context.Cart
        //                .FirstOrDefaultAsync(m => m.ProductId == id && m.Customers.Email == User.Identity.Name);
        //            if (Carts != null)
        //            {
        //                Carts.Quantity = Carts.Quantity + 1;
        //                _context.Update(Carts);
        //                await _context.SaveChangesAsync();
        //                TempData["cart_action"] = "Add";
        //                TempData["message"] = "Item Successfully Added";
        //                return RedirectToAction("Index", "Carts");
        //            }
        //            else if (Carts == null)
        //            {
        //                cart.ProductId = id;
        //                var product = await _context.Products
        //                    .FirstOrDefaultAsync(m => m.ProductId == id);
        //                cart.Products = product;
        //                var customer = await _context.Customers
        //                    .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
        //                cart.CustomerId = customer.CustomerId;
        //                cart.Customers = await _context.Customers
        //                    .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
        //                cart.Addition_Date = DateTime.Now;
        //                var transaction = await _context.Transactions
        //                    .FirstOrDefaultAsync(m => m.CartId == cart.CartId);
        //                cart.Quantity = 1;
        //                _context.Add(cart);
        //                await _context.SaveChangesAsync();
        //                TempData["cart_action"] = "Add";
        //                TempData["message"] = "Item Successfully Added";

        //                return RedirectToAction("Index", "Carts");
        //            }

        //            return RedirectToAction("Index", "Carts");
        //        }

        //        public IActionResult Index()
        //        {
        //            return View();
        //        }

        //        public IActionResult Privacy()
        //        {
        //            return View();
        //        }

        //        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //        public IActionResult Error()
        //        {
        //            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //        }
    }
}
