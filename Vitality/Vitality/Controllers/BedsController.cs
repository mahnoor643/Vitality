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
    public class BedsController : Controller
    {
        private readonly VitalitydbContext _context;

        public BedsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: Beds
        public async Task<IActionResult> Index(int searchresult)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {

                // Retrieve all beds if no search result is provided
                if (searchresult == 0)
            {
                var allBeds = await _context.Beds.ToListAsync();
                return View(allBeds);
            }

            // Filter beds based on the provided search result
            var beds = _context.Beds.Where(x => x.BedId == searchresult);

            // Check if _context.Beds is null
            if (beds == null)
            {
                return Problem("Entity set 'VitalitydbContext.Beds' is null.");
            }

            // Retrieve and return the filtered results
            var bedList = await beds.ToListAsync();
            return View(bedList);
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: Beds/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: Beds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedId,Status,Bed1,BedAmount")] Bed bed)
        {
            if (ModelState.IsValid)
            {
                bed.Status = 0;
                _context.Add(bed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bed);
        }

        // GET: Beds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Beds == null)
            {
                return NotFound();
            }

            var bed = await _context.Beds.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }
            return View(bed);
        }

        // POST: Beds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BedId,Status,Bed1,BedAmount")] Bed bed)
        {
            if (id != bed.BedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Beds.Find(bed.BedId);
                    if (bed.Status!= null)
                    {
                        data.Status=bed.Status;
                    }
                    if(bed.BedAmount != null)
                    {
                        data.BedAmount=bed.BedAmount;
                    }
                    if(bed.Bed1!= null)
                    {
                        data.Bed1 = bed.Bed1;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedExists(bed.BedId))
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
            return View(bed);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try { 
            if (_context.Beds == null)
            {
                return Problem("Entity set 'VitalitydbContext.Beds'  is null.");
            }
            var bed = await _context.Beds.FindAsync(id);
            if (bed != null)
            {
                _context.Beds.Remove(bed);
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

        private bool BedExists(int id)
        {
          return (_context.Beds?.Any(e => e.BedId == id)).GetValueOrDefault();
        }

        //Deactivating Bed from admin
        public IActionResult Deactive(int id)
        {

            var bedDeactive = _context.Beds.FirstOrDefault(c => c.BedId == id);

            if (bedDeactive != null)
            {
                bedDeactive.Status = 0;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Activating Bed from admin

        /*public IActionResult Active(int id)
        {

            var bedActive = _context.Beds.FirstOrDefault(c => c.BedId == id);

            if (bedActive != null)
            {
                bedActive.Status = 1;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }*/
    }
}