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
    public class EmergencyPatientsController : Controller
    {
        private readonly VitalitydbContext _context;

        public EmergencyPatientsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: EmergencyPatients
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.EmergencyPatients != null ?
                        View(await _context.EmergencyPatients.ToListAsync()) :
                        Problem("Entity set 'VitalitydbContext.EmergencyPatients'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: EmergencyPatients/Create
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

        // POST: EmergencyPatients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmergencyPatientsId,EmergencyPatientsName,AmountPayable,AmountPaid,Status")] EmergencyPatient emergencyPatient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emergencyPatient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emergencyPatient);
        }

        // GET: EmergencyPatients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmergencyPatients == null)
            {
                return NotFound();
            }

            var emergencyPatient = await _context.EmergencyPatients.FindAsync(id);
            if (emergencyPatient == null)
            {
                return NotFound();
            }
            return View(emergencyPatient);
        }

        // POST: EmergencyPatients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmergencyPatientsId,EmergencyPatientsName,AmountPayable,AmountPaid,Status")] EmergencyPatient emergencyPatient)
        {
            if (id != emergencyPatient.EmergencyPatientsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.EmergencyPatients.Find(emergencyPatient.EmergencyPatientsId);
                    if (emergencyPatient.EmergencyPatientsName != null)
                    {
                        data.EmergencyPatientsName = emergencyPatient.EmergencyPatientsName;
                    }
                    if (emergencyPatient.AmountPayable != null)
                    {
                        data.AmountPayable = emergencyPatient.AmountPayable;
                    }
                    if (emergencyPatient.AmountPaid != null)
                    {
                        data.AmountPaid = emergencyPatient.AmountPaid;
                    }
                    if (emergencyPatient.Status != null)
                    {
                        data.Status = emergencyPatient.Status;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmergencyPatientExists(emergencyPatient.EmergencyPatientsId))
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
            return View(emergencyPatient);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.EmergencyPatients == null)
                {
                    return Problem("Entity set 'VitalitydbContext.EmergencyPatients'  is null.");
                }
                var emergencyPatient = await _context.EmergencyPatients.FindAsync(id);
                if (emergencyPatient != null)
                {
                    _context.EmergencyPatients.Remove(emergencyPatient);
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

        private bool EmergencyPatientExists(int id)
        {
            return (_context.EmergencyPatients?.Any(e => e.EmergencyPatientsId == id)).GetValueOrDefault();
        }

        //Deactivating EmergencyPatients from admin
        public IActionResult Deactive(int id)
        {

            var Deactive = _context.EmergencyPatients.FirstOrDefault(c => c.EmergencyPatientsId == id);

            if (Deactive != null)
            {
                Deactive.Status = 0;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Activating EmergencyPatients from admin

        public IActionResult Active(int id)
        {

            var Active = _context.EmergencyPatients.FirstOrDefault(c => c.EmergencyPatientsId == id);

            if (Active != null)
            {
                Active.Status = 1;
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
