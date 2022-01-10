using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covaciu_Carla_Proiect.Data;
using Covaciu_Carla_Proiect.Models;
using Microsoft.AspNetCore.Authorization;

namespace Covaciu_Carla_Proiect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class PhonesController : Controller
    {
        private readonly StoreContext _context;

        public PhonesController(StoreContext context)
        {
            _context = context;
        }

        // GET: Phones
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ModelSortParm"] = String.IsNullOrEmpty(sortOrder) ? "model_desc" : "";
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
            var phones = from b in _context.Phones
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                phones = phones.Where(s => s.Model.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "model_desc":
                    phones = phones.OrderByDescending(b => b.Model);
                    break;
                case "Price":
                    phones = phones.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    phones = phones.OrderByDescending(b => b.Price);
                    break;
                default:
                    phones = phones.OrderBy(b => b.Model);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Phone>.CreateAsync(phones.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Phones/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones.Include(s => s.Orders).ThenInclude(e => e.Customer).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (phone == null)
            {
                return NotFound();
            }

            return View(phone);
        }

        // GET: Phones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Phones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Company,Price")] Phone phone)
        {
            try
            {
                if (ModelState.IsValid)
            {
                _context.Add(phone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            return View(phone);
        }

        // GET: Phones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return NotFound();
            }
            return View(phone);
        }

        // POST: Phones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Phones.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Phone>(studentToUpdate,"",s => s.Company, s => s.Model, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Phones/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (phone == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }

            return View(phone);
        }

        // POST: Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PhoneExists(int id)
        {
            return _context.Phones.Any(e => e.ID == id);
        }
    }
}
