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
    public class PatientPaymentsController : Controller
    {
        private readonly VitalitydbContext _context;

        public PatientPaymentsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: PatientPayments
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.PatientPayments.Include(p => p.PatientsCard);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }



        // GET: PatientPayments/Create
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

        // POST: PatientPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Pay,PatientsCardId")] PatientPayment patientPayment)
        {

            patientPayment.PatientsCardId = id;
            _context.Add(patientPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", patientPayment.PatientsCardId);
            return View(patientPayment);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.PatientPayments == null)
                {
                    return Problem("Entity set 'VitalitydbContext.PatientPayments'  is null.");
                }
                var patientPayment = await _context.PatientPayments.FindAsync(id);
                if (patientPayment != null)
                {
                    _context.PatientPayments.Remove(patientPayment);
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

        private bool PatientPaymentExists(int id)
        {
            return (_context.PatientPayments?.Any(e => e.PayId == id)).GetValueOrDefault();
        }

        //Deduct
        public IActionResult deduct(int id)
        {
            var payID = _context.PatientPayments.FirstOrDefault(e => e.PayId == id);
            if (payID != null)
            {
                var amount = payID.Pay;
                var patientC = payID.PatientsCardId;
                var patientCard = _context.PatientsIdcards.FirstOrDefault(c => c.PatientsCardId == patientC);
                if (patientCard != null)
                {
                    payID.Status = 1;
                    patientCard.PayableAmount -= amount;
                    _context.SaveChanges();
                    return RedirectToAction("Index", "PatientsIDCards");
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
