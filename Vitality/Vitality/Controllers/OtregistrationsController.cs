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
    public class OtregistrationsController : Controller
    {
        private readonly VitalitydbContext _context;

        public OtregistrationsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: Otregistrations
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.Otregistrations.Where(x => x.Status == 0).Include(o => o.Doctor).Include(o => o.OttimeNavigation).Include(o => o.PatientsCard);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        //Scheduled Operations
        public async Task<IActionResult> OTRegistered()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.Otregistrations.Where(x => x.Status == 1).Include(o => o.Doctor).Include(o => o.OttimeNavigation).Include(o => o.PatientsCard);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: Otregistrations/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var doctor = _context.DoctorsRegistrations.Where(x => x.Status == 1 || x.Status == 3).ToList();
                ViewData["DoctorId"] = new SelectList(doctor, "DoctorsId", "DoctorsName");
                ViewData["Ottime"] = new SelectList(_context.OttimeSlots, "OttimeId", "Ottime");
                ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: Otregistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientsOtid,PatientsCardId,Ottime,Otdate,DoctorId,Status")] Otregistration otregistration)
        {
            _context.Add(otregistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            var doctor = _context.DoctorsRegistrations.Where(x => x.Status == 1 || x.Status == 3).ToList();
            ViewData["DoctorId"] = new SelectList(doctor, "DoctorsId", "DoctorsName", otregistration.DoctorId);
            ViewData["Ottime"] = new SelectList(_context.OttimeSlots, "OttimeId", "Ottime", otregistration.Ottime);
            ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", otregistration.PatientsCardId);
            return View(otregistration);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.Otregistrations == null)
                {
                    return Problem("Entity set 'VitalitydbContext.Otregistrations'  is null.");
                }
                var otregistration = await _context.Otregistrations.FindAsync(id);
                if (otregistration != null)
                {
                    _context.Otregistrations.Remove(otregistration);
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

        //Payment add
        public ActionResult addPayment(int id)
        {
            var OT = _context.Otregistrations.Where(x => x.PatientsOtid == id).FirstOrDefault();
            if (OT != null)
            {
                var card = _context.PatientsIdcards.Where(x => x.PatientsCardId == OT.PatientsCardId).FirstOrDefault();
                card.PayableAmount += 50000;
                OT.Status = 1;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(OTRegistered));
        }
        private bool OtregistrationExists(int id)
        {
            return (_context.Otregistrations?.Any(e => e.PatientsOtid == id)).GetValueOrDefault();
        }
    }
}
