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
    public class PatientsIdcardsController : Controller
    {
        private readonly VitalitydbContext _context;

        public PatientsIdcardsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: PatientsIdcards
        public async Task<IActionResult> Index(string searchresult)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.PatientsIdcards.Include(p => p.Patients)
                .Where(x => (x.Patients.PatientsName.Contains(searchresult) || searchresult == null) ||
                (x.PatientsCardId.ToString().Contains(searchresult) || searchresult == null) ||
                (x.Patients.PatientsEmail.Contains(searchresult) || searchresult == null));
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }

        }

        // GET: PatientsIdcards/Create
        public IActionResult Create(int? id)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: PatientsIdcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("PatientsCardId,PatientsId,AdvancePayment,Status,ValidDate,PayableAmount")] PatientsIdcard patientsIdcard)
        {
            patientsIdcard.Status = 1;
            patientsIdcard.PatientsId = id;
            _context.Add(patientsIdcard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", patientsIdcard.PatientsId);
            return View(patientsIdcard);
        }

        // GET: PatientsIdcards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientsIdcards == null)
            {
                return NotFound();
            }

            var patientsIdcard = await _context.PatientsIdcards.FindAsync(id);
            if (patientsIdcard == null)
            {
                return NotFound();
            }
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", patientsIdcard.PatientsId);
            return View(patientsIdcard);
        }

        // POST: PatientsIdcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientsCardId,PatientsId,AdvancePayment,Status,ValidDate,PayableAmount")] PatientsIdcard patientsIdcard)
        {
            if (id != patientsIdcard.PatientsCardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientsIdcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientsIdcardExists(patientsIdcard.PatientsCardId))
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
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", patientsIdcard.PatientsId);
            return View(patientsIdcard);
        }

        // GET: PatientsIdcards/Delete/5
        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.PatientsIdcards == null)
                {
                    return Problem("Entity set 'VitalitydbContext.PatientsIdcards'  is null.");
                }
                var patientsIdcard = await _context.PatientsIdcards.FindAsync(id);
                if (patientsIdcard != null)
                {
                    _context.PatientsIdcards.Remove(patientsIdcard);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You Can not delete it, Cause ID Card is in use!";
                return RedirectToAction("Index");
            }
        }

        private bool PatientsIdcardExists(int id)
        {
            return (_context.PatientsIdcards?.Any(e => e.PatientsCardId == id)).GetValueOrDefault();
        }

        //Deactivating Doctors from admin
        public IActionResult Deactive(int id)
        {

            var patientCardDeactive = _context.PatientsIdcards.FirstOrDefault(c => c.PatientsCardId == id);

            if (patientCardDeactive != null)
            {
                patientCardDeactive.Status = 0;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Activating Doctors fromm admin

        public IActionResult Active(int id)
        {

            var patientCardActive = _context.PatientsIdcards.FirstOrDefault(c => c.PatientsCardId == id);

            if (patientCardActive != null)
            {
                patientCardActive.Status = 1;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }



    }
}
