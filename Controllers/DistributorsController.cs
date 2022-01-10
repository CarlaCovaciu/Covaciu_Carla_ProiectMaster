using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covaciu_Carla_Proiect.Data;
using Covaciu_Carla_Proiect.Models;
using Covaciu_Carla_Proiect.Models.StoreViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Covaciu_Carla_Proiect.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DistributorsController : Controller
    {
        private readonly StoreContext _context;

        public DistributorsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Distributors
        public async Task<IActionResult> Index(int? id, int? phoneID)
        {
            var viewModel = new DistributorIndexData();
            viewModel.Distributors = await _context.Distributors
            .Include(i => i.DistributedPhones)
            .ThenInclude(i => i.Phone)
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
                viewModel.Phones = distributor.DistributedPhones.Select(s => s.Phone);
            }
            if (phoneID != null)
            {
                ViewData["PhoneID"] = phoneID.Value;
                viewModel.Orders = viewModel.Phones.Where(
                x => x.ID == phoneID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Distributors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
        public async Task<IActionResult> Create([Bind("ID,DistributorName,Adress")] Distributor distributor)
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
            if (id == null)
            {
                return NotFound();
            }

            var distributor = await _context.Distributors
            .Include(i => i.DistributedPhones).ThenInclude(i => i.Phone)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            if (distributor == null)
            {
                return NotFound();
            }
            PopulateDistributedPhoneData(distributor);
            return View(distributor);
        }

        private void PopulateDistributedPhoneData(Distributor distributor)
        {
            var allPhones = _context.Phones;
            var distributorPhones = new HashSet<int>(distributor.DistributedPhones.Select(c => c.PhoneID));
            var viewModel = new List<DistributedPhoneData>();
            foreach (var phone in allPhones)
            {
                viewModel.Add(new DistributedPhoneData
                {
                    PhoneID = phone.ID,
                    Model = phone.Model,
                    IsDistributed = distributorPhones.Contains(phone.ID)
                });
            }
            ViewData["Phones"] = viewModel;
        }

        // POST: Distributors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPhones)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distributorToUpdate = await _context.Distributors
            .Include(i => i.DistributedPhones)
            .ThenInclude(i => i.Phone)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Distributor>(
            distributorToUpdate,"", i => i.DistributorName, i => i.Adress))
            {
                UpdateDistributedPhones(selectedPhones, distributorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateDistributedPhones(selectedPhones, distributorToUpdate);
            PopulateDistributedPhoneData(distributorToUpdate);
            return View(distributorToUpdate);
        }
        private void UpdateDistributedPhones(string[] selectedPhones, Distributor distributorToUpdate)
        {
            if (selectedPhones == null)
            {
                distributorToUpdate.DistributedPhones = new List<DistributedPhone>();
                return;
            }
            var selectedPhonesHS = new HashSet<string>(selectedPhones);
            var distributedPhones = new HashSet<int>
            (distributorToUpdate.DistributedPhones.Select(c => c.Phone.ID));
            foreach (var phone in _context.Phones)
            {
                if (selectedPhonesHS.Contains(phone.ID.ToString()))
                {
                    if (!distributedPhones.Contains(phone.ID))
                    {
                        distributorToUpdate.DistributedPhones.Add(new DistributedPhone { DistributorID = distributorToUpdate.ID, PhoneID = phone.ID });
                    }
                }
                else
                {
                    if (distributedPhones.Contains(phone.ID))
                    {
                        DistributedPhone phoneToRemove = distributorToUpdate.DistributedPhones.FirstOrDefault(i => i.PhoneID == phone.ID);
                        _context.Remove(phoneToRemove);
                    }
                }
            }
        }

        // GET: Distributors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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
            var distributor = await _context.Distributors.FindAsync(id);
            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributorExists(int id)
        {
            return _context.Distributors.Any(e => e.ID == id);
        }
    }
}
