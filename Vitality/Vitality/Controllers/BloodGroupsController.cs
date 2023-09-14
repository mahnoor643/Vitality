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
    public class BloodGroupsController : Controller
    {
        private readonly VitalitydbContext _context;

        public BloodGroupsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: BloodGroups
        public async Task<IActionResult> Index()
        {
              return _context.BloodGroups != null ? 
                          View(await _context.BloodGroups.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.BloodGroups'  is null.");
        }

        // GET: BloodGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups
                .FirstOrDefaultAsync(m => m.BloodGroupId == id);
            if (bloodGroup == null)
            {
                return NotFound();
            }

            return View(bloodGroup);
        }

        // GET: BloodGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BloodGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BloodGroupId,BloodGroup1")] BloodGroup bloodGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloodGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bloodGroup);
        }

        // GET: BloodGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups.FindAsync(id);
            if (bloodGroup == null)
            {
                return NotFound();
            }
            return View(bloodGroup);
        }

        // POST: BloodGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BloodGroupId,BloodGroup1")] BloodGroup bloodGroup)
        {
            if (id != bloodGroup.BloodGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloodGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodGroupExists(bloodGroup.BloodGroupId))
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
            return View(bloodGroup);
        }

        // GET: BloodGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups
                .FirstOrDefaultAsync(m => m.BloodGroupId == id);
            if (bloodGroup == null)
            {
                return NotFound();
            }

            return View(bloodGroup);
        }

        // POST: BloodGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BloodGroups == null)
            {
                return Problem("Entity set 'VitalitydbContext.BloodGroups'  is null.");
            }
            var bloodGroup = await _context.BloodGroups.FindAsync(id);
            if (bloodGroup != null)
            {
                _context.BloodGroups.Remove(bloodGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodGroupExists(int id)
        {
          return (_context.BloodGroups?.Any(e => e.BloodGroupId == id)).GetValueOrDefault();
        }
    }
}
