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
    public class OtdaySlotsController : Controller
    {
        private readonly VitalitydbContext _context;

        public OtdaySlotsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: OtdaySlots
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.OtdaySlots != null ?
                          View(await _context.OtdaySlots.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.OtdaySlots'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }

        }


        // GET: OtdaySlots/Create
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

        // POST: OtdaySlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OtdayId,Otday")] OtdaySlot otdaySlot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(otdaySlot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(otdaySlot);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.OtdaySlots == null)
                {
                    return Problem("Entity set 'VitalitydbContext.OtdaySlots'  is null.");
                }
                var otdaySlot = await _context.OtdaySlots.FindAsync(id);
                if (otdaySlot != null)
                {
                    _context.OtdaySlots.Remove(otdaySlot);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You Can not delete it, Cause This day is Alloted!";
                return RedirectToAction("Index");
            }
        }

        private bool OtdaySlotExists(int id)
        {
            return (_context.OtdaySlots?.Any(e => e.OtdayId == id)).GetValueOrDefault();
        }
    }
}
