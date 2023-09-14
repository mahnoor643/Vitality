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
    public class OttimeSlotsController : Controller
    {
        private readonly VitalitydbContext _context;

        public OttimeSlotsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: OttimeSlots
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.OttimeSlots != null ?
                          View(await _context.OttimeSlots.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.OttimeSlots'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: OttimeSlots/Create
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

        // POST: OttimeSlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OttimeId,Ottime")] OttimeSlot ottimeSlot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ottimeSlot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ottimeSlot);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.OttimeSlots == null)
                {
                    return Problem("Entity set 'VitalitydbContext.OttimeSlots' is null.");
                }

                var ottimeSlot = await _context.OttimeSlots.FindAsync(id);
                if (ottimeSlot != null)
                {
                    _context.OttimeSlots.Remove(ottimeSlot);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You Can not delete it, Cause This time is Alloted!";
                return RedirectToAction("Index");
            }
        }


        private bool OttimeSlotExists(int id)
        {
            return (_context.OttimeSlots?.Any(e => e.OttimeId == id)).GetValueOrDefault();
        }
    }
}
