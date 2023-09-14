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
    public class FeedbacksController : Controller
    {
        private readonly VitalitydbContext _context;
        List<string> sessionInfo = new List<string>();

        public FeedbacksController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.Feedbacks != null ?
                        View(await _context.Feedbacks.ToListAsync()) :
                        Problem("Entity set 'VitalitydbContext.Feedbacks'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,Feedback1,UserName,UserEmail")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["ErrorMessage"] = "Your feedback is added!";
                return RedirectToAction("Index", "Home");
            }
            return View(feedback);
        }

        //User Create
        // GET: Feedbacks/Create
        public IActionResult UserCreate()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCreate([Bind("FeedbackId,Feedback1,UserName,UserEmail")] Feedback feedback)
        {

            if (HttpContext.Session.GetInt32(SessionVariables.SessionPatientsID) != null)
            {
                var patientID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionPatientsID));
                var patient = _context.PatientsRegistrations.Where(x => x.PatientsId == patientID).FirstOrDefault();
                if (patient != null)
                {
                    feedback.UserEmail = patient.PatientsEmail;
                    feedback.UserName = patient.PatientsName;
                    _context.Add(feedback);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("dashboard", "PatientsRegistrations");
            }
            else
            {
                return RedirectToAction("Login", "PatientsRegistrations");
            }
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.Feedbacks == null)
                {
                    return Problem("Entity set 'VitalitydbContext.Feedbacks'  is null.");
                }
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback != null)
                {
                    _context.Feedbacks.Remove(feedback);
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

        private bool FeedbackExists(int id)
        {
            return (_context.Feedbacks?.Any(e => e.FeedbackId == id)).GetValueOrDefault();
        }

        //Activating feedback to fetch on index
        public IActionResult Active(int id)
        {

            var Active = _context.Feedbacks.FirstOrDefault(c => c.FeedbackId == id);

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
        public IActionResult Deactive(int id)
        {

            var Active = _context.Feedbacks.FirstOrDefault(c => c.FeedbackId == id);

            if (Active != null)
            {
                Active.Status = 0;
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
