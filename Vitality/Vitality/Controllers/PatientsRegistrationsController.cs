using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class PatientsRegistrationsController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public PatientsRegistrationsController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: PatientsRegistrations
        public async Task<IActionResult> Index(string searchresult)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["cardidfk"] = _context.PatientsIdcards
                .Select(x => x.PatientsId).ToList();

                var patientsReg = _context.PatientsRegistrations.Where(x => x.PatientsName.Contains(searchresult) || searchresult == null);
                return View(await patientsReg.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        //Patient's Dashboard
        public IActionResult dashboard()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionPatientsID) != null)
            {
                var patientID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionPatientsID));
                var patientCard = _context.PatientsIdcards.Where(c => c.PatientsId == patientID).ToList();
                var ViewModel = new patientDashboard
                {
                    PatientsRegistration = _context.PatientsRegistrations.Where(x => x.PatientsId == patientID).ToList(),
                    PatientsCard = _context.PatientsIdcards.Where(y => y.PatientsId == patientID).ToList(),
                    Feedback = _context.Feedbacks.ToList(),
                    DoctorsAppointment = _context.DoctorsAppointments.Include(d => d.DoctorAppointmentNavigation).Include(d => d.DoctorAppointmentTimeNavigation).Include(d => d.Patients).Where(x => x.PatientsId == patientID).ToList(),
                };
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "PatientsRegistrations");
            }
        }

        // GET: PatientsRegistrations/Create
        public IActionResult Create()
        {
            return View();

        }

        // POST: PatientsRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientsId,PatientsName,PatientsEmail,PatientsPhoneNo,PatientsPwd")] PatientsRegistration patientsRegistration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientsRegistration);
                await _context.SaveChangesAsync();
                TempData["ErrorMessage"] = "You have registered your account successfully, Now Login with your new Account..";
                return RedirectToAction("Login", "PatientsRegistrations");
            }
            return View(patientsRegistration);
        }

        // GET: PatientsRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientsRegistrations == null)
            {
                return NotFound();
            }

            var patientsRegistration = await _context.PatientsRegistrations.FindAsync(id);
            if (patientsRegistration == null)
            {
                return NotFound();
            }
            return View(patientsRegistration);
        }

        // POST: PatientsRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientsId,PatientsName,PatientsEmail,PatientsPhoneNo,PatientsPwd")] PatientsRegistration patientsRegistration)
        {
            if (id != patientsRegistration.PatientsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.PatientsRegistrations.Find(patientsRegistration.PatientsId);
                    if (patientsRegistration.PatientsName != null)
                    {
                        data.PatientsName = patientsRegistration.PatientsName;
                    }
                    if (patientsRegistration.PatientsPwd != null)
                    {
                        data.PatientsPwd = patientsRegistration.PatientsPwd;
                    }
                    if (patientsRegistration.PatientsEmail != null)
                    {
                        data.PatientsEmail = patientsRegistration.PatientsEmail;
                    }
                    if (patientsRegistration.PatientsPhoneNo != null)
                    {
                        data.PatientsPhoneNo = patientsRegistration.PatientsPhoneNo;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientsRegistrationExists(patientsRegistration.PatientsId))
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
            return View(patientsRegistration);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.PatientsRegistrations == null)
                {
                    return Problem("Entity set 'VitalitydbContext.PatientsRegistrations'  is null.");
                }
                var patientsRegistration = await _context.PatientsRegistrations.FindAsync(id);
                if (patientsRegistration != null)
                {
                    _context.PatientsRegistrations.Remove(patientsRegistration);
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

        private bool PatientsRegistrationExists(int id)
        {
            return (_context.PatientsRegistrations?.Any(e => e.PatientsId == id)).GetValueOrDefault();
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(PatientsRegistration login)
        {
            var login_info = _context.PatientsRegistrations.Where(x => x.PatientsEmail == login.PatientsEmail && x.PatientsPwd == login.PatientsPwd)
                .FirstOrDefault();
            if (login_info != null)
            {
                HttpContext.Session.SetInt32(SessionVariables.SessionPatientsID, login_info.PatientsId);
                var patientId = HttpContext.Session.GetInt32(SessionVariables.SessionPatientsID);
                string patient = patientId?.ToString() ?? string.Empty;
                sessionInfo.Add(patient);
                return RedirectToAction("dashboard", "PatientsRegistrations");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToAction("Login");
            }
        }

        //Logout session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionVariables.SessionPatientsID); // Remove specific item from the session

            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }
    }
}
