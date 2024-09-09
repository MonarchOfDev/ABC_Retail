using ABC_Retail_v3.Data;
using ABC_Retail_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics;


namespace ABC_Retail_v3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult About_Us()
        {
            return View();
        }
        public IActionResult Contact_Us()
        {
            return View();
        }
        public async Task<IActionResult> Our_Work(string search)
        {

            if (search != null)
            {
                var products = await _context.Products.Where(x => x.Product_name.StartsWith(search) || x.Price.ToString().StartsWith(search)).ToListAsync();
                return View(products);

            }
            return View(await _context.Products.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Our_Work(int id, Cart cart)
        {
            var Carts = await _context.Cart
                .FirstOrDefaultAsync(m => m.Product_id == id && m.Customers.Email == User.Identity.Name);
            if (Carts != null)
            {
                Carts.Quantity = Carts.Quantity + 1;
                _context.Update(Carts);
                await _context.SaveChangesAsync();
                TempData["cart_action"] = "Add";
                TempData["message"] = "Item Successfully Added";
                return RedirectToAction("Index", "Carts");
            }
            else if (Carts == null)
            {
                cart.Product_id = id;
                var product = await _context.Products
                    .FirstOrDefaultAsync(m => m.Product_id == id);
                cart.Products = product;
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
                cart.Customer_id = customer.Customer_id;
                cart.Customers = await _context.Customers
                    .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
                cart.Addition_Date = DateTime.Now;
                var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(m => m.CartId == cart.CartId);
                cart.Quantity = 1;
                _context.Add(cart);
                await _context.SaveChangesAsync();
                TempData["cart_action"] = "Add";
                TempData["message"] = "Item Successfully Added";

                return RedirectToAction("Index", "Carts");
            }

            return RedirectToAction("Index", "Carts");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
