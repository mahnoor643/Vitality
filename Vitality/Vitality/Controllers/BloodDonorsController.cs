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
    public class BloodDonorsController : Controller
    {
        private readonly VitalitydbContext _context;

        public BloodDonorsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: BloodDonors
        public async Task<IActionResult> Index(string searchresult)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.BloodDonors.Where(a => a.Status == 0).Include(b => b.BloodGroupNavigation)
                .Where(x => (x.BloodGroupNavigation.BloodGroup1.Contains(searchresult) || searchresult == null));
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: BloodDonors/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {

                ViewData["BloodGroup"] = new SelectList(_context.BloodGroups, "BloodGroupId", "BloodGroup1");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: BloodDonors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BloodDonorsId,BloodGroup,BloodDonorsName,BloodDonorsEmail,BloodDonorsPhoneNo,Status")] BloodDonor bloodDonor)
        {
            bloodDonor.Status = 0;
            _context.Add(bloodDonor);
            await _context.SaveChangesAsync();

            ViewData["BloodGroup"] = new SelectList(_context.BloodGroups, "BloodGroupId", "BloodGroup1", bloodDonor.BloodGroup);
            return RedirectToAction(nameof(Index));
        }

        // GET: BloodDonors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BloodDonors == null)
            {
                return NotFound();
            }

            var bloodDonor = await _context.BloodDonors.FindAsync(id);
            if (bloodDonor == null)
            {
                return NotFound();
            }
            ViewData["BloodGroup"] = new SelectList(_context.BloodGroups, "BloodGroupId", "BloodGroup1", bloodDonor.BloodGroup);
            return View(bloodDonor);
        }

        // POST: BloodDonors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BloodDonorsId,BloodGroup,BloodDonorsName,BloodDonorsEmail,BloodDonorsPhoneNo,Status")] BloodDonor bloodDonor)
        {
            if (id != bloodDonor.BloodDonorsId)
            {
                return NotFound();
            }


            try
            {
                _context.Update(bloodDonor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloodDonorExists(bloodDonor.BloodDonorsId))
                {
                    return NotFound();
                }
                else
                {
                    var existing = _context.BloodDonors.Find(bloodDonor.BloodDonorsId);
                    if (bloodDonor.BloodGroup != null)
                    {
                        existing.BloodGroup = bloodDonor.BloodGroup;
                    }
                    if (bloodDonor.BloodDonorsName != null)
                    {
                        existing.BloodDonorsName = bloodDonor.BloodDonorsName;
                    }
                    if (bloodDonor.BloodDonorsEmail != null)
                    {
                        existing.BloodDonorsEmail = bloodDonor.BloodDonorsEmail;
                    }
                    if (bloodDonor.BloodDonorsPhoneNo != null)
                    {
                        existing.BloodDonorsPhoneNo = bloodDonor.BloodDonorsPhoneNo;
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            ViewData["BloodGroup"] = new SelectList(_context.BloodGroups, "BloodGroupId", "BloodGroup1", bloodDonor.BloodGroup);
            return View(bloodDonor);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.BloodDonors == null)
                {
                    return Problem("Entity set 'VitalitydbContext.BloodDonors'  is null.");
                }
                var bloodDonor = await _context.BloodDonors.FindAsync(id);
                if (bloodDonor != null)
                {
                    _context.BloodDonors.Remove(bloodDonor);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You can't delete cause this data is used!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool BloodDonorExists(int id)
        {
            return (_context.BloodDonors?.Any(e => e.BloodDonorsId == id)).GetValueOrDefault();
        }
    }
}
