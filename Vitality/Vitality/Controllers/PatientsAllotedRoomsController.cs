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
    public class PatientsAllotedRoomsController : Controller
    {
        private readonly VitalitydbContext _context;

        public PatientsAllotedRoomsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: PatientsAllotedRooms
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.PatientsAllotedRooms.Where(x => x.Status == 0).Include(p => p.PatientsCard).Include(p => p.PatientsRoom);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: PatientsAllotedRooms/Create
        public IActionResult Create(int? id)
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId");
                ViewData["PatientsRoomId"] = new SelectList(_context.PatientRooms, "RoomId", "RoomId");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: PatientsAllotedRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("PatientsAllotedRoomId,PatientsRoomId,CurrentDateTime,Days,PatientsCardId,Status,AllotTill")] PatientsAllotedRoom patientsAllotedRoom)
        {
            patientsAllotedRoom.PatientsRoomId = id;
            patientsAllotedRoom.Days = 0;
            _context.Add(patientsAllotedRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /*ViewData["PatientsId"] = new SelectList(_context.PatientsRegistrations, "PatientsId", "PatientsId", patientsAllotedRoom.PatientsId);
            ViewData["PatientsCardId"] = new SelectList(_context.PatientsIdcards, "PatientsCardId", "PatientsCardId", patientsAllotedRoom.PatientsCardId);
            ViewData["PatientsRoomId"] = new SelectList(_context.PatientRooms, "RoomId", "RoomId", patientsAllotedRoom.PatientsRoomId);
            return View(patientsAllotedRoom);*/
        }

        //payment add for room
        public IActionResult payment(int id)
        {

            var roomAllotPayment = _context.PatientsAllotedRooms.FirstOrDefault(c => c.PatientsAllotedRoomId == id);

            if (roomAllotPayment != null)
            {
                DateTime currentDate = DateTime.Today;
                var futureDate = roomAllotPayment.AllotTill;

                TimeSpan timeDifference = (TimeSpan)(futureDate - currentDate);
                int days = timeDifference.Days;

                //Activating bed
                roomAllotPayment.Status = 1;
                roomAllotPayment.Days = days;
                //Adding amount in patients card id
                var card = roomAllotPayment.PatientsCardId;
                var addPayment = _context.PatientsIdcards.Where(x => x.PatientsCardId == card).FirstOrDefault();
                var room = roomAllotPayment.PatientsRoomId;
                var roomAmount = _context.PatientRooms.Where(x => x.RoomId == room).FirstOrDefault();
                if (roomAmount != null)
                {
                    roomAmount.Status = 1;
                    var roomAmountWithRespectDays = (int)roomAmount.RoomAmount * days;
                    addPayment.PayableAmount += roomAmountWithRespectDays;
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
                if (_context.PatientsAllotedRooms == null)
                {
                    return Problem("Entity set 'VitalitydbContext.PatientsAllotedRooms'  is null.");
                }
                var patientsAllotedRoom = await _context.PatientsAllotedRooms.FindAsync(id);
                if (patientsAllotedRoom != null)
                {
                    _context.PatientsAllotedRooms.Remove(patientsAllotedRoom);
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

        private bool PatientsAllotedRoomExists(int id)
        {
            return (_context.PatientsAllotedRooms?.Any(e => e.PatientsAllotedRoomId == id)).GetValueOrDefault();
        }
    }
}
