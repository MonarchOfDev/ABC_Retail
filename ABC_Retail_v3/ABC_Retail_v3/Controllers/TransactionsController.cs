//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//using ABC_Retail_v3.Models;

//namespace ABC_Retail_v3.Controllers
//{
//    public class TransactionsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public TransactionsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Transactions
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Transactions.Include(t => t.CartId).Include(t => t.CustomerId).Include(t => t.ProductId);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Transactions/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var transactions = await _context.Transactions
//                .Include(t => t.CartId)
//                .Include(t => t.CustomerId)
//                .Include(t => t.ProductId)
//                .FirstOrDefaultAsync(m => m.TransactionId == id);
//            if (transactions == null)
//            {
//                return NotFound();
//            }

//            return View(transactions);
//        }

//        // GET: Transactions/Create
//        public IActionResult Create()
//        {
//            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId");
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
//            return View();
//        }

//        // POST: Transactions/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("TransactionId,CartId,PurchaseDate,Price,ProductId,CustomerId,OrderStatus")] Transactions transactions)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(transactions);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", transactions.CartId);
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", transactions.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", transactions.ProductId);
//            return View(transactions);
//        }

//        // GET: Transactions/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var transactions = await _context.Transactions.FindAsync(id);
//            if (transactions == null)
//            {
//                return NotFound();
//            }
//            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", transactions.CartId);
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", transactions.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", transactions.ProductId);
//            return View(transactions);
//        }

//        // POST: Transactions/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,CartId,PurchaseDate,Price,ProductId,CustomerId,OrderStatus")] Transactions transactions)
//        {
//            if (id != transactions.TransactionId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(transactions);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!TransactionsExists(transactions.TransactionId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", transactions.CartId);
//            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", transactions.CustomerId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", transactions.ProductId);
//            return View(transactions);
//        }

//        // GET: Transactions/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var transactions = await _context.Transactions
//                .Include(t => t.CartId)
//                .Include(t => t.CustomerId)
//                .Include(t => t.ProductId)
//                .FirstOrDefaultAsync(m => m.TransactionId == id);
//            if (transactions == null)
//            {
//                return NotFound();
//            }

//            return View(transactions);
//        }

//        // POST: Transactions/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var transactions = await _context.Transactions.FindAsync(id);
//            if (transactions != null)
//            {
//                _context.Transactions.Remove(transactions);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool TransactionsExists(int id)
//        {
//            return _context.Transactions.Any(e => e.TransactionId == id);
//        }
//    }
//}
