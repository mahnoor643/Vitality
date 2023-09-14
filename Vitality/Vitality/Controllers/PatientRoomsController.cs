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
    public class PatientRoomsController : Controller
    {
        private readonly VitalitydbContext _context;

        public PatientRoomsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: PatientRooms
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.PatientRooms != null ?
                          View(await _context.PatientRooms.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.PatientRooms'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }

        }

        // GET: PatientRooms/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: PatientRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Room,Status,RoomAmount")] PatientRoom patientRoom)
        {
            if (ModelState.IsValid)
            {
                patientRoom.Status = 0;
                _context.Add(patientRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patientRoom);
        }

        // GET: PatientRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientRooms == null)
            {
                return NotFound();
            }

            var patientRoom = await _context.PatientRooms.FindAsync(id);
            if (patientRoom == null)
            {
                return NotFound();
            }
            return View(patientRoom);
        }

        // POST: PatientRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Room,Status,RoomAmount")] PatientRoom patientRoom)
        {
            if (id != patientRoom.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientRoomExists(patientRoom.RoomId))
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
            return View(patientRoom);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.PatientRooms == null)
                {
                    return Problem("Entity set 'VitalitydbContext.PatientRooms'  is null.");
                }
                var patientRoom = await _context.PatientRooms.FindAsync(id);
                if (patientRoom != null)
                {
                    _context.PatientRooms.Remove(patientRoom);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You Can not delete it, Cause This Room is Alloted!";
                return RedirectToAction("Index");
            }
        }

        //Deactivating Bed from admin
        public IActionResult Deactive(int id)
        {

            var roomDeactive = _context.PatientRooms.FirstOrDefault(c => c.RoomId == id);

            if (roomDeactive != null)
            {
                roomDeactive.Status = 0;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PatientRoomExists(int id)
        {
            return (_context.PatientRooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
