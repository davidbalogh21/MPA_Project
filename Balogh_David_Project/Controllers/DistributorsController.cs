using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Balogh_David_Project.Data;
using Balogh_David_Project.Models;
using Balogh_David_Project.Models.LaptopViewModels;


namespace Balogh_David_Project
{
    public class DistributorsController : Controller
    {
        private readonly LaptopContext _context;

        public DistributorsController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Distributors
        public async Task<IActionResult> Index(int? id, int? laptopID)
        {
            var viewModel = new DistributorsIndexData();
            viewModel.Distributors = await _context.Distributors
            .Include(i => i.DistributedLaptops)
            .ThenInclude(i => i.Laptop)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.DistributorName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["DistributorID"] = id.Value;
                Distributor distributor = viewModel.Distributors.Where(
                i => i.ID == id.Value).Single();
                viewModel.Laptops = distributor.DistributedLaptops.Select(s => s.Laptop);
            }
            if (laptopID != null)
            {
                ViewData["LaptopID"] = laptopID.Value;
                viewModel.Orders = viewModel.Laptops.Where(
                x => x.ID == laptopID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Distributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // GET: Distributors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distributors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DistributorName,Address")] Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distributor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distributor);
        }

        // GET: Distributors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
             .Include(i => i.DistributedLaptops).ThenInclude(i => i.Laptop)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);
            if (distributor == null)
            {
                return NotFound();
            }
            PopulateDistributedLaptopData(distributor);
            return View(distributor);
        }
        private void PopulateDistributedLaptopData(Distributor distributor)
        {
            var allLaptops = _context.Laptops;
            var distributorLaptops = new HashSet<int>(distributor.DistributedLaptops.Select(c => c.LaptopID));
            var viewModel = new List<DistributedLaptopData>();
            foreach (var laptop in allLaptops)
            {
                viewModel.Add(new DistributedLaptopData
                {
                    LaptopID = laptop.ID,
                    Name = laptop.Name,
                    IsPublished = distributorLaptops.Contains(laptop.ID)
                });
            }
            ViewData["Laptops"] = viewModel;
        }

        // POST: Distributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedLaptops)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distributorToUpdate = await _context.Distributors
            .Include(i => i.DistributedLaptops)
            .ThenInclude(i => i.Laptop)
            .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Distributor>(
            distributorToUpdate,
            "",
            i => i.DistributorName, i => i.Address))
            {
                UpdatePublishedBooks(selectedLaptops, distributorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedBooks(selectedLaptops, distributorToUpdate);
            PopulateDistributedLaptopData(distributorToUpdate);
            return View(distributorToUpdate);
        }

        private void UpdatePublishedBooks(string[] selectedLaptops, Distributor distributorToUpdate)
        {
            if (selectedLaptops == null)
            {
                distributorToUpdate.DistributedLaptops = new List<DistributedLaptop>();
                return;
            }
            var selectedLaptopHS = new HashSet<string>(selectedLaptops);
            var distributedLaptops = new HashSet<int>
            (distributorToUpdate.DistributedLaptops.Select(c => c.Laptop.ID));
            foreach (var laptop in _context.Laptops)
            {
                if (selectedLaptopHS.Contains(laptop.ID.ToString()))
                {
                    if (!distributedLaptops.Contains(laptop.ID))
                    {
                        distributorToUpdate.DistributedLaptops.Add(new DistributedLaptop
                        {
                            DistributorID =
                       distributorToUpdate.ID,
                            LaptopID = laptop.ID
                        });
                    }
                }
                else
                {
                    if (distributedLaptops.Contains(laptop.ID))
                    {
                        DistributedLaptop laptopToRemove = distributorToUpdate.DistributedLaptops.FirstOrDefault(i
                       => i.LaptopID == laptop.ID);
                        _context.Remove(laptopToRemove);
                    }
                }
            }
        }

        // GET: Distributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Distributors == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distributor == null)
            {
                return NotFound();
            }

            return View(distributor);
        }

        // POST: Distributors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Distributors == null)
            {
                return Problem("Entity set 'LaptopContext.Distributors'  is null.");
            }
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor != null)
            {
                _context.Distributors.Remove(distributor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
          return (_context.Distributors?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
