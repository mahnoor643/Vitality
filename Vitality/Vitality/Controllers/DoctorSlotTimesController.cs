using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vitality.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class DoctorSlotTimesController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public DoctorSlotTimesController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: DoctorSlotTimes
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) == null)
            {
                return RedirectToAction("login", "DoctorsRegistrations");
            }
            else
            {
                int doctorID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID));
                var vitalitydbContext = _context.DoctorSlotTimes.Include(d => d.Doctors).Where(x => x.DoctorsId == doctorID);
                return View(await vitalitydbContext.ToListAsync());
            }
        }

        // GET: DoctorSlotTimes/Create
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

        // POST: DoctorSlotTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorSlotTimeId,DoctorSlotTime1,DoctorsId")] DoctorSlotTime doctorSlotTime)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) != null)
            {
                // The session value for SessionDoctorsID is not null
                doctorSlotTime.DoctorsId = (int)HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID);
                _context.Add(doctorSlotTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.DoctorSlotTimes == null)
                {
                    return Problem("Entity set 'VitalitydbContext.DoctorSlotTimes'  is null.");
                }
                var doctorSlotTime = await _context.DoctorSlotTimes.FindAsync(id);
                if (doctorSlotTime != null)
                {
                    _context.DoctorSlotTimes.Remove(doctorSlotTime);
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

        private bool DoctorSlotTimeExists(int id)
        {
            return (_context.DoctorSlotTimes?.Any(e => e.DoctorSlotTimeId == id)).GetValueOrDefault();
        }
    }
}
