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
    public class AvailedBloodsController : Controller
    {
        private readonly VitalitydbContext _context;

        public AvailedBloodsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: AvailedBloods
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.AvailedBloods.Include(a => a.PatientsCard).Include(b => b.BloodDonors);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: AvailedBloods/Create
        public IActionResult Create(int? id)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: AvailedBloods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("BloodRecieverId,BloodDonorsId,PatientsCardId,BloodStatus")] AvailedBlood availedBlood)
        {
            availedBlood.BloodStatus = 1;
            availedBlood.BloodDonorsId = id;
            _context.Add(availedBlood);

            var bloodDonor = _context.BloodDonors.FirstOrDefault(c => c.BloodDonorsId == id);
            if (bloodDonor != null)
            {
                bloodDonor.Status = 1;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AvailedBloods/Edit/5
        /* public async Task<IActionResult> Edit(int? id)
         {
             if (id == null || _context.AvailedBloods == null)
             {
                 return NotFound();
             }

             var availedBlood = await _context.AvailedBloods.FindAsync(id);
             if (availedBlood == null)
             {
                 return NotFound();
             }
             ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", availedBlood.PatientsCardId);
             return View(availedBlood);
         }*/

        // POST: AvailedBloods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BloodRecieverId,BloodDonorsId,PatientsCardId")] AvailedBlood availedBlood)
        {
            if (id != availedBlood.BloodRecieverId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(availedBlood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvailedBloodExists(availedBlood.BloodRecieverId))
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
            ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", availedBlood.PatientsCardId);
            return View(availedBlood);
        }*/

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.AvailedBloods == null)
                {
                    return Problem("Entity set 'VitalitydbContext.AvailedBloods'  is null.");
                }
                var availedBlood = await _context.AvailedBloods.FindAsync(id);
                if (availedBlood != null)
                {
                    _context.AvailedBloods.Remove(availedBlood);
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

        private bool AvailedBloodExists(int id)
        {
            return (_context.AvailedBloods?.Any(e => e.BloodRecieverId == id)).GetValueOrDefault();
        }
    }
}
