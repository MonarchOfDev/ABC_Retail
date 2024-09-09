using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ABC_Retail_v3.Data;
using ABC_Retail_v3.Models;

namespace ABC_Retail_v3.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var applicationDbContext = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Check_Out(int id)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var applicationDbContext = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.Customer_id == id);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Check_Purchase(int id, int price)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            Transactions transactions = new Transactions();
            var Cart = _context.Cart.Include(c => c.Customers).Include(c => c.Products).Where(c => c.Customers.Customer_id == id);
            foreach (var item in Cart)
            {
                transactions.CartId = item.CartId;
                transactions.Purchase_date = DateTime.Now;
                transactions.Price = item.Products.Price;
                transactions.Product_id = item.Product_id;
                transactions.Customer_id = item.Customer_id;
                transactions.Order_Status = "Order Confirmed";
                //transactions.CartId1= item.CartId;
            }

            TempData["cart_action"] = "Order";
            TempData["message"] = "Order Successfully Confirm";
            return View("Index", Cart);
        }

        //Check_Purchase

        //// GET: Carts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Cart
        //        .Include(c => c.Customers)
        //        .Include(c => c.Products)
        //        .FirstOrDefaultAsync(m => m.CartId == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["Customer_id"] = new SelectList(_context.Customers, "Customer_id", "Email");
            ViewData["Product_id"] = new SelectList(_context.Products, "Product_id", "Product_id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,Customer_id,Product_id,Quantity,Addition_Date")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customer_id"] = new SelectList(_context.Customers, "Customer_id", "Email", cart.Customer_id);
            ViewData["Product_id"] = new SelectList(_context.Products, "Product_id", "Product_id", cart.Product_id);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["Customer_id"] = new SelectList(_context.Customers, "Customer_id", "Email", cart.Customer_id);
            ViewData["Product_id"] = new SelectList(_context.Products, "Product_id", "Product_id", cart.Product_id);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,Customer_id,Product_id,Quantity,Addition_Date")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(cart);
                await _context.SaveChangesAsync();

                TempData["cart_action"] = "Edit";
                TempData["message"] = "Item Successfully Modified";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.CartId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["Customer_id"] = new SelectList(_context.Customers, "Customer_id", "Email", cart.Customer_id);
            ViewData["Product_id"] = new SelectList(_context.Products, "Product_id", "Product_id", cart.Product_id);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customers)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();

            TempData["cart_action"] = "Delete";
            TempData["message"] = "Item Successfully Removed";

            return RedirectToAction("Index", "Carts");
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }
    }
}
