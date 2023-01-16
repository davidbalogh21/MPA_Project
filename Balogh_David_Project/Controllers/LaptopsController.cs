using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Balogh_David_Project.Data;
using Balogh_David_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Balogh_David_Project
{
    [Authorize(Roles = "Employee")]
    public class LaptopsController : Controller
    {
        private readonly LaptopContext _context;

        public LaptopsController(LaptopContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Laptops
        public async Task<IActionResult> Index(
  string sortOrder,
  string currentFilter,
  string searchString,
  int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var laptops = from b in _context.Laptops
                        .Include(a => a.Manufacturer)
                          select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                laptops = laptops.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                laptops = laptops.Include(a => a.Manufacturer);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    laptops = laptops.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    laptops = laptops.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    laptops = laptops.OrderByDescending(b => b.Price);
                    break;
                default:
                    laptops = laptops.OrderBy(b => b.Name);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Laptop>.CreateAsync(laptops.AsNoTracking(), pageNumber ??
           1, pageSize));
        }


        // GET: Laptops/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Laptops == null)
            {
                return NotFound();
            }


            var laptop = await _context.Laptops
    .Include(b => b.Manufacturer)
 .Include(s => s.Orders)
 .ThenInclude(e => e.Customer)
 .AsNoTracking()
 .FirstOrDefaultAsync(m => m.ID == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Laptops/Create
        public IActionResult Create()
        {
            ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "Name");
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Manufacturer,ManufacturerID,Price")] Laptop laptop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(laptop);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(laptop);
        }

        // GET: Laptops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Laptops == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "Name", laptop.ManufacturerID);
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ManufacturerID,Price")] Laptop laptop)
        {
            if (id != laptop.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerID"] = new SelectList(_context.Manufacturers, "ID", "ID", laptop.ManufacturerID);
            return View(laptop);
        }

        // GET: Laptops/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var laptop = await _context.Laptops
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (laptop == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }
            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);

            if (laptop == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Laptops.Remove(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool LaptopExists(int id)
        {
            return (_context.Laptops?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
