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
    public class HospitalPharmacyPrescriptionsController : Controller
    {
        private readonly VitalitydbContext _context;

        public HospitalPharmacyPrescriptionsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: HospitalPharmacyPrescriptions
        public async Task<IActionResult> Index()
        {
            var vitalitydbContext = _context.HospitalPharmacyPrescriptions.Where(h=>h.Status=="pending").Include(h => h.Patients);
            return View(await vitalitydbContext.ToListAsync());
        }

        
        // GET: HospitalPharmacyPrescriptions/Create
        public IActionResult Create()
        {
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId");
            return View();
        }

        // POST: HospitalPharmacyPrescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrescriptionId,PatientsId,Medicines,Status")] HospitalPharmacyPrescription hospitalPharmacyPrescription)
        {
            _context.Add(hospitalPharmacyPrescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", hospitalPharmacyPrescription.PatientsId);
            return View(hospitalPharmacyPrescription);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.HospitalPharmacyPrescriptions == null)
            {
                return Problem("Entity set 'VitalitydbContext.HospitalPharmacyPrescriptions'  is null.");
            }
            var hospitalPharmacyPrescription = await _context.HospitalPharmacyPrescriptions.FindAsync(id);
            if (hospitalPharmacyPrescription != null)
            {
                _context.HospitalPharmacyPrescriptions.Remove(hospitalPharmacyPrescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalPharmacyPrescriptionExists(int id)
        {
          return (_context.HospitalPharmacyPrescriptions?.Any(e => e.PrescriptionId == id)).GetValueOrDefault();
        }

        //recieved medicine
        public async Task<IActionResult> medicineRecieved()
        {
            var vitalitydbContext = _context.HospitalPharmacyPrescriptions.Include(h => h.Patients);
            return View(await vitalitydbContext.ToListAsync());
        }

        //UpdateStatus
        public IActionResult UpdateStatus(int id)
        {
            var status = _context.HospitalPharmacyPrescriptions.FirstOrDefault(c => c.PrescriptionId == id);

            if (status != null)
            {
                status.Status = "recieved";
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
