using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vitality.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class DoctorSlotDaysController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public DoctorSlotDaysController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: DoctorSlotDays
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) == null)
            {
                return RedirectToAction("login", "DoctorsRegistrations");
            }
            else
            {
                int doctorID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID));
                var vitalitydbContext = _context.DoctorSlotDays.Include(d => d.Doctors).Where(x => x.DoctorsId == doctorID);
                return View(await vitalitydbContext.ToListAsync());
            }
        }

        // GET: DoctorSlotDays/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) == null)
            {
                return RedirectToAction("login", "DoctorsRegistrations");
            }
            else
            {
                return View();
            }
        }

        // POST: DoctorSlotDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorSlotDaysId,DoctorSlotDays,DoctorsId")] DoctorSlotDay doctorSlotDay)
        {
            /*if (ModelState.IsValid)
            {*/
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) != null)
            {
                // The session value for SessionDoctorsID is not null
                doctorSlotDay.DoctorsId = (int)HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID);
                _context.Add(doctorSlotDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        //Delete functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.DoctorSlotDays == null)
                {
                    return Problem("Entity set 'VitalitydbContext.DoctorSlotDays'  is null.");
                }
                var doctorSlotDay = await _context.DoctorSlotDays.FindAsync(id);
                if (doctorSlotDay != null)
                {
                    _context.DoctorSlotDays.Remove(doctorSlotDay);
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

        private bool DoctorSlotDayExists(int id)
        {
            return (_context.DoctorSlotDays?.Any(e => e.DoctorSlotDaysId == id)).GetValueOrDefault();
        }
    }
}
