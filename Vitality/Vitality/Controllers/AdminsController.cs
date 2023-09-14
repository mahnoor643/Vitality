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
    public class AdminsController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public AdminsController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.Admins != null ? 
                          View(await _context.Admins.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.Admins'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

       
        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,AdminUsername,AdminPwd,AdminName")] Admin admin)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,AdminUsername,AdminPwd,AdminName")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Admins.Find(admin.AdminId);
                    if (admin.AdminName != null)
                    {
                        data.AdminName = admin.AdminName;
                    }
                    if (admin.AdminUsername != null)
                    {
                        data.AdminUsername = admin.AdminUsername;
                    }
                    if (admin.AdminPwd != null)
                    {
                        data.AdminPwd = admin.AdminPwd;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
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
            return View(admin);
        }

         //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.Admins == null)
            {
                return Problem("Entity set 'VitalitydbContext.Admins'  is null.");
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
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

        private bool AdminExists(int id)
        {
          return (_context.Admins?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }

        //Login page
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Admin login)
        {
            var login_info = _context.Admins.Where(x => x.AdminUsername == login.AdminUsername && x.AdminPwd == login.AdminPwd).FirstOrDefault();
            if (login_info != null)
            {
                HttpContext.Session.SetInt32(SessionVariables.SessionAdminID, login_info.AdminId);
                var adminId = HttpContext.Session.GetInt32(SessionVariables.SessionAdminID);
                string admin = adminId?.ToString() ?? string.Empty;
                sessionInfo.Add(admin);
                return RedirectToAction("dashboard", "Admins");
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
            HttpContext.Session.Remove(SessionVariables.SessionAdminID); // Remove specific item from the session

            return RedirectToAction("Login", "Admins"); // Redirect to home page after logout
        }

        //Admin Dashboard
        public IActionResult dashboard()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var adminID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionAdminID));
                var Variable_Name = DateTime.Now.Date; // Use DateTime type
                var ViewModel = new adminDashboard
                {
                    PatientsRegistrations=_context.PatientsRegistrations.ToList(),
                    DoctorsAppointments=_context.DoctorsAppointments.Include(x=>x.DoctorAppointmentTimeNavigation).Where(x=>x.DoctorAppointmentDate== Variable_Name).ToList(),
                    DoctorsRegistration=_context.DoctorsRegistrations.Where(x=>x.Status==3||x.Status==4||x.Status==1).ToList(),
                    feedbacks=_context.Feedbacks.ToList(),
                    PatientsRegistrationsCount = _context.PatientsRegistrations.Count(),
                    DoctorsAppointmentsCount = _context.DoctorsAppointments.Count(),
                    DoctorsRegistrationCount = _context.DoctorsRegistrations.Where(x => x.Status == 3 || x.Status == 4 || x.Status == 1).Count(),
                    FeedbacksCount = _context.Feedbacks.Count(),
                };
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }
    }
}
