using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class HospitalBillingsController : Controller
    {
        private readonly VitalitydbContext _context;

        public HospitalBillingsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: HospitalBillings
        public async Task<IActionResult> Index()
        {
              return _context.HospitalBillings != null ? 
                          View(await _context.HospitalBillings.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.HospitalBillings'  is null.");
        }

        // GET: HospitalBillings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HospitalBillings == null)
            {
                return NotFound();
            }

            var hospitalBilling = await _context.HospitalBillings
                .FirstOrDefaultAsync(m => m.HospitalBillings == id);
            if (hospitalBilling == null)
            {
                return NotFound();
            }

            return View(hospitalBilling);
        }

        // GET: HospitalBillings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HospitalBillings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalBillings,BillFor,Status")] HospitalBilling hospitalBilling)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalBilling);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalBilling);
        }

        // GET: HospitalBillings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HospitalBillings == null)
            {
                return NotFound();
            }

            var hospitalBilling = await _context.HospitalBillings.FindAsync(id);
            if (hospitalBilling == null)
            {
                return NotFound();
            }
            return View(hospitalBilling);
        }

        // POST: HospitalBillings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalBillings,BillFor,Status")] HospitalBilling hospitalBilling)
        {
            if (id != hospitalBilling.HospitalBillings)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalBilling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalBillingExists(hospitalBilling.HospitalBillings))
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
            return View(hospitalBilling);
        }

        // GET: HospitalBillings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HospitalBillings == null)
            {
                return NotFound();
            }

            var hospitalBilling = await _context.HospitalBillings
                .FirstOrDefaultAsync(m => m.HospitalBillings == id);
            if (hospitalBilling == null)
            {
                return NotFound();
            }

            return View(hospitalBilling);
        }

        // POST: HospitalBillings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HospitalBillings == null)
            {
                return Problem("Entity set 'VitalitydbContext.HospitalBillings'  is null.");
            }
            var hospitalBilling = await _context.HospitalBillings.FindAsync(id);
            if (hospitalBilling != null)
            {
                _context.HospitalBillings.Remove(hospitalBilling);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalBillingExists(int id)
        {
          return (_context.HospitalBillings?.Any(e => e.HospitalBillings == id)).GetValueOrDefault();
        }
    }
}
