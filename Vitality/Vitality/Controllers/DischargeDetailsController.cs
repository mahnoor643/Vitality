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
    public class DischargeDetailsController : Controller
    {
        private readonly VitalitydbContext _context;

        public DischargeDetailsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: DischargeDetails
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.DischargeDetails.Include(d => d.DischargePatientsCard);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: DischargeDetails/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["DischargePatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: DischargeDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DischargeId,DischargePatientsCardId,DischargeFrom")] DischargeDetail dischargeDetail)
        {
            if (dischargeDetail != null)
            {
                _context.Add(dischargeDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DischargePatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", dischargeDetail.DischargePatientsCardId);
            return View(dischargeDetail);
        }

        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.DischargeDetails == null)
                {
                    return Problem("Entity set 'VitalitydbContext.DischargeDetails'  is null.");
                }
                var dischargeDetail = await _context.DischargeDetails.FindAsync(id);
                if (dischargeDetail != null)
                {
                    _context.DischargeDetails.Remove(dischargeDetail);
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

        private bool DischargeDetailExists(int id)
        {
            return (_context.DischargeDetails?.Any(e => e.DischargeId == id)).GetValueOrDefault();
        }
    }
}
