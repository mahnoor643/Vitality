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
    public class BedAllotmentsController : Controller
    {
        private readonly VitalitydbContext _context;

        public BedAllotmentsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: BedAllotments
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.BedAllotments.Where(c => c.Status == 0).Include(b => b.Beds).Include(b => b.PatientsCardNoNavigation);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        //Alloted Beds
        public async Task<IActionResult> allotedBeds()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.BedAllotments.Where(c => c.Status == 1).Include(b => b.Beds).Include(b => b.PatientsCardNoNavigation);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: BedAllotments/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["BedsId"] = new SelectList(_context.Beds, "BedId", "BedId");
                ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId");
                ViewData["PatientsCardNo"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: BedAllotments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("BedAllotmentId,BedsId,CurrentDateTime,Days,PatientsId,PatientsCardNo,AllotTill")] BedAllotment bedAllotment)
        {
            bedAllotment.BedsId = id;
            bedAllotment.Days = 0;
            _context.Add(bedAllotment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /* ViewData["BedsId"] = new SelectList(_context.Beds, "BedId", "BedId", bedAllotment.BedsId);
             ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", bedAllotment.PatientsId);
             ViewData["PatientsCardNo"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", bedAllotment.PatientsCardNo);
             return View(bedAllotment);*/
        }

        //payment add for bed
        public IActionResult payment(int id)
        {

            var bedAllotPayment = _context.BedAllotments.FirstOrDefault(c => c.BedAllotmentId == id);

            if (bedAllotPayment != null)
            {
                DateTime currentDate = DateTime.Today;
                var futureDate = bedAllotPayment.AllotTill;

                TimeSpan timeDifference = (TimeSpan)(futureDate - currentDate);
                int days = timeDifference.Days;

                //Activating bed
                bedAllotPayment.Status = 1;
                bedAllotPayment.Days = days;
                //Adding amount in patients card id
                var card = bedAllotPayment.PatientsCardNo;
                var addPayment = _context.PatientsIdcards.Where(x => x.PatientsCardId == card).FirstOrDefault();
                var bed = bedAllotPayment.BedsId;
                var bedAmount = _context.Beds.Where(x => x.BedId == bed).FirstOrDefault();
                if (bedAmount != null)
                {
                    bedAmount.Status = 1;
                    var bedAmountWithRespectDays = (int)bedAmount.BedAmount * days;
                    addPayment.PayableAmount += bedAmountWithRespectDays;
                }

                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.BedAllotments == null)
                {
                    return Problem("Entity set 'VitalitydbContext.BedAllotments'  is null.");
                }
                var bedAllotment = await _context.BedAllotments.FindAsync(id);
                if (bedAllotment != null)
                {
                    _context.BedAllotments.Remove(bedAllotment);
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

        private bool BedAllotmentExists(int id)
        {
            return (_context.BedAllotments?.Any(e => e.BedAllotmentId == id)).GetValueOrDefault();
        }
    }
}
