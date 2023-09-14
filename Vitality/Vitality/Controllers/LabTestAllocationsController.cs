using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vitality.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Vitality.Controllers
{
    public class LabTestAllocationsController : Controller
    {
        private readonly VitalitydbContext _context;

        public LabTestAllocationsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: LabTestAllocations
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.LabTestAllocations.Where(s => s.Status == 0).Include(l => l.LabTest).Include(l => l.PatientsCard);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: LabTestAllocations/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["LabTestId"] = new SelectList(_context.LabTests, "LabTestId", "LabTest1");
                ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: LabTestAllocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabTestAllocationId,LabTestId,PatientsCardId,CurrentDateTime")] LabTestAllocation labTestAllocation)
        {
            _context.Add(labTestAllocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewData["LabTestId"] = new SelectList(_context.LabTests, "LabTestId", "LabTest1", labTestAllocation.LabTestId);
            ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", labTestAllocation.PatientsCardId);
            return View(labTestAllocation);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.LabTestAllocations == null)
                {
                    return Problem("Entity set 'VitalitydbContext.LabTestAllocations'  is null.");
                }
                var labTestAllocation = await _context.LabTestAllocations.FindAsync(id);
                if (labTestAllocation != null)
                {
                    _context.LabTestAllocations.Remove(labTestAllocation);
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

        private bool LabTestAllocationExists(int id)
        {
            return (_context.LabTestAllocations?.Any(e => e.LabTestAllocationId == id)).GetValueOrDefault();
        }

        //Add  payment in patients card id
        public IActionResult addPayment(int id)
        {
            var LabAllocationId = _context.LabTestAllocations.Where(x => x.LabTestAllocationId == id).FirstOrDefault();
            var lab = LabAllocationId.LabTestId;
            var LabTId = _context.LabTests.Where(x => x.LabTestId == lab).FirstOrDefault();
            if (LabAllocationId != null)
            {
                var amount = LabTId.LabTestAmount;
                var patientscardID = LabAllocationId.PatientsCardId;
                var patient = _context.PatientsIdcards.Where(a => a.PatientsCardId == patientscardID).FirstOrDefault();

                if (patient != null)
                {
                    // Add the new payment amount to the existing payment
                    patient.PayableAmount += (int)amount;

                    LabAllocationId.Status = 1;
                    // Save changes to the database
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
