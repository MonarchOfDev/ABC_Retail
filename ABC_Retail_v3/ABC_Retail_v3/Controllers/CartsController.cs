//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//using ABC_Retail_v3.Models;
//using Azure.Data.Tables;

//namespace ABC_Retail_v3.Controllers
//{
//    public class CartsController : Controller
//    {

//        private readonly TableServiceClient _tableServiceClient;
//        private readonly ILogger<AccountController> _accountLogger;

//        public CartsController(ILogger<AccountController> logger, TableServiceClient tableServiceClient)
//        {
//            _tableServiceClient = tableServiceClient;
//            _accountLogger = logger;
//        }
//        public async Task CreateTableAsync(string tableName)
//        {
//            var tableClient = _tableServiceClient.GetTableClient(tableName);
//            await tableClient.CreateIfNotExistsAsync();
//        }
//        // GET: Carts
//        public async Task<IActionResult> Index()
//        {
//            CreateTableAsync("Cart");
//            //if (User.Identity.Name == null)
//            //{
//            //    return RedirectToAction("Login","Account");
//            //}

//            var applicationDbContext = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.Email == User.Identity.Name);
//            return View(await applicationDbContext.ToListAsync());
//        }
//        public async Task<IActionResult> Check_Out(int id)
//        {
//            if (User.Identity.Name == null)
//            {
//                return RedirectToPage("/Account/Login", new { area = "Identity" });
//            }

//            var applicationDbContext = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.CustomerId == id);
//            return View(await applicationDbContext.ToListAsync());
//        }
//        public async Task<IActionResult> Check_Purchase(int id, int price)
//        {
//            if (User.Identity.Name == null)
//            {
//                return RedirectToPage("/Account/Login", new { area = "Identity" });
//            }
//            Transactions transactions = new Transactions();
//            var Cart = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.CustomerId == id);
//            foreach (var item in Cart)
//            {
//                transactions.CartId = item.CartId;
//                transactions.PurchaseDate = DateTime.Now;
//                transactions.Price = item.Products.Price;
//                transactions.ProductId = item.ProductId;
//                transactions.CustomerId = item.CustomerId;
//                transactions.OrderStatus = "Order Confirmed";
//                //transactions.CartId1= item.CartId;
//            }

//            TempData["cart_action"] = "Order";
//            TempData["message"] = "Order Successfully Confirm";
//            return View("Index", Cart);
//        }

//        //Check_Purchase

//        //// GET: Carts/Details/5
//        //public async Task<IActionResult> Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    var cart = await _context.Cart
//        //        .Include(c => c.Customers)
//        //        .Include(c => c.Products)
//        //        .FirstOrDefaultAsync(m => m.CartId == id);
//        //    if (cart == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    return View(cart);
//        //}

//        // GET: Carts/Create
//        public IActionResult Create()
//        {
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
//            return View();
//        }

//        // POST: Carts/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("CartId,CustomerId,ProductId,Quantity,Addition_Date")] Cart cart)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(cart);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", cart.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
//            return View(cart);
//        }

//        // GET: Carts/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var cart = await _context.Cart.FindAsync(id);
//            if (cart == null)
//            {
//                return NotFound();
//            }
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", cart.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
//            return View(cart);
//        }

//        // POST: Carts/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("CartId,CustomerId,ProductId,Quantity,Addition_Date")] Cart cart)
//        {
//            if (id != cart.CartId)
//            {
//                return NotFound();
//            }

//            try
//            {
//                _context.Update(cart);
//                await _context.SaveChangesAsync();

//                TempData["cart_action"] = "Edit";
//                TempData["message"] = "Item Successfully Modified";
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!CartExists(cart.CartId))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction(nameof(Index));

//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", cart.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
//            return View(cart);
//        }

//        // GET: Carts/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var cart = await _context.Cart
//                .Include(c => c.Customers)
//                .Include(c => c.Products)
//                .FirstOrDefaultAsync(m => m.CartId == id);
//            if (cart == null)
//            {
//                return NotFound();
//            }

//            return View(cart);
//        }

//        // POST: Carts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var cart = await _context.Cart.FindAsync(id);
//            if (cart != null)
//            {
//                _context.Cart.Remove(cart);
//            }

//            await _context.SaveChangesAsync();

//            TempData["cart_action"] = "Delete";
//            TempData["message"] = "Item Successfully Removed";

//            return RedirectToAction("Index", "Carts");
//        }

//        private bool CartExists(int id)
//        {
//            return _context.Cart.Any(e => e.CartId == id);
//        }
//    }
//}
