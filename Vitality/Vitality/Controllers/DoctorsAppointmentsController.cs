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
    public class DoctorsAppointmentsController : Controller
    {
        private readonly VitalitydbContext _context;

        public DoctorsAppointmentsController(VitalitydbContext context)
        {
            _context = context;
        }
        //Get SlotDay
        [HttpGet]
        public JsonResult getSlotTime(int? getid)
        {
            /*  _context.Configuration.ProxyCreationEnabled = false;*/
            List<DoctorSlotTime> slotList = new List<DoctorSlotTime>();
            slotList = _context.DoctorSlotTimes.Where(x => x.DoctorsId == getid).ToList();
            return Json(slotList);
        }
        // GET: DoctorsAppointments
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.DoctorsAppointments.Include(d => d.DoctorAppointmentNavigation).Include(d => d.DoctorAppointmentTimeNavigation).Include(d => d.Patients);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }



        // GET: DoctorsAppointments/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var doctor = _context.DoctorsRegistrations.Where(x => x.Status == 1 || x.Status == 3).ToList();
                ViewData["DoctorAppointment"] = new SelectList(doctor, "DoctorsId", "DoctorsName");
                ViewData["DoctorAppointmentTime"] = new SelectList(_context.DoctorSlotTimes, "DoctorSlotTimeId", "DoctorSlotTime1");
                ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId");
                return View();
                
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: DoctorsAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorAppointmentId,DoctorAppointmentDate,DoctorAppointmentTime,DoctorAppointment,Status,PatientsId")] DoctorsAppointment doctorsAppointment)
        {
            if (doctorsAppointment != null)
            {
                try
                {

                    _context.Add(doctorsAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                TempData["ErrorMessage"] = "You can not access this page right now.";
                return RedirectToAction(nameof(Index));
            }
        }
            var doctor=_context.DoctorsRegistrations.Where(x=>x.Status==1 || x.Status==3).ToList();
            ViewData["DoctorAppointment"] = new SelectList(doctor, "DoctorsId", "DoctorsName", doctorsAppointment.DoctorAppointment);
            ViewData["DoctorAppointmentTime"] = new SelectList(_context.DoctorSlotTimes, "DoctorSlotTimeId", "DoctorSlotTime1", doctorsAppointment.DoctorAppointmentTime);
            ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", doctorsAppointment.PatientsId);
            return View(doctorsAppointment);
        }

        //Delete Functionality

        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.DoctorsAppointments == null)
                {
                    return Problem("Entity set 'VitalitydbContext.DoctorsAppointments'  is null.");
                }
                var doctorsAppointment = await _context.DoctorsAppointments.FindAsync(id);
                if (doctorsAppointment != null)
                {
                    _context.DoctorsAppointments.Remove(doctorsAppointment);
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

        private bool DoctorsAppointmentExists(int id)
        {
            return (_context.DoctorsAppointments?.Any(e => e.DoctorAppointmentId == id)).GetValueOrDefault();
        }

        //Confirming Appointment here
        public IActionResult confirmAppointment(int id)
        {

            var appointmentConfirm = _context.DoctorsAppointments.FirstOrDefault(c => c.DoctorAppointmentId == id);

            if (appointmentConfirm != null)
            {
                appointmentConfirm.Status = "Confirmed";
                _context.SaveChanges();
                TempData["ErrorMessage"] = "Your Appointment is Confirmed!";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
