using System;
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
using ABC_Retail_v3.AzureStorageService.Service;
using ABC_Retail_v3.AzureStorageService.Interface;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using ABC_Retail_v3.AzureQueueService.Interface;


namespace ABC_Retail_v3.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ILogger<HomeController> _accountLogger;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IQueueStorageService _queueStorageService;
        private const string conn = "DefaultEndpointsProtocol=https;AccountName=abcdb;AccountKey=9s2Ze/6LC3CqZC6POmRyAlk2sh9xT0dQxwRJq9sKHbaDaVo1FEA1D1KhTIqxjS6+y6N1wBOZ6Hv4+AStlA4TUw==;QueueEndpoint=https://abcdb.queue.core.windows.net";
        private const string qname = "registeruser";

        private readonly QueueClient queueClient = new QueueClient(conn, qname);

        public HomeController(ILogger<HomeController> logger, TableServiceClient tableServiceClient, IBlobStorageService blobStorageService, IFileStorageService fileStorageService,
    IQueueStorageService queueStorageService)
        {
            _tableServiceClient = tableServiceClient;
            _accountLogger = logger;
            _blobStorageService = blobStorageService;
            _fileStorageService = fileStorageService;
            _queueStorageService = queueStorageService;
        }

        public void qCreate() { 
           queueClient.CreateIfNotExists();
           return;
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

        [HttpGet]
        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var fileName = file.FileName;
                        var fileUrl = await _fileStorageService.UploadFileAsync(fileName, stream);
                        ViewData["FileUrl"] = fileUrl;
                        try {
                            qCreate();
                            string message = "File Uploaded - "+ fileName;
                            queueClient.SendMessage(message);
                            _accountLogger.LogInformation($"Message added: {message}");
                        } catch (Exception ex) {
                            throw;
                        }
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
                _accountLogger.LogInformation($"Error in UploadProductImage: {ex.Message}");
                ViewData["Error"] = "An error occurred while uploading the file. Please try again.";
            }

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
                        try
                        {
                            qCreate();
                            string message = "Image Uploaded - "+ fileName;
                            queueClient.SendMessage(message);
                            _accountLogger.LogInformation($"Message added: {message}");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
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
                _accountLogger.LogInformation($"Error in UploadProductImage: {ex.Message}");
                ViewData["Error"] = "An error occurred while uploading the file. Please try again.";
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadProductImage(string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    var stream = await _blobStorageService.DownloadFileAsync(fileName);
                    return File(stream, "application/octet-stream", fileName);
                }
                else
                {
                    return BadRequest("Filename is missing.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DownloadProductImage: {ex.Message}");
                _accountLogger.LogError($"Error in DownloadProductImage: {ex.Message}");
                return StatusCode(500, "An error occurred while downloading the file.");
            }
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
                _accountLogger.LogInformation($"Error fetching users: {ex.Message}");
                return StatusCode(500, "Error retrieving users.");
            }

            return View(craftProducts.ToList());

        }
        private async Task EnqueueMessageAsync(string message)
        {
            try
            {
                await _queueStorageService.SendMessageAsync(message);
                _accountLogger.LogInformation($"Message added to queue: {message}");
            }
            catch (Exception ex)
            {
                _accountLogger.LogError($"Failed to enqueue message: {ex.Message}");
            }
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
