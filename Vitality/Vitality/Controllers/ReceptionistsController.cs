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
    public class ReceptionistsController : Controller
    {
        private readonly VitalitydbContext _context;

        public ReceptionistsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: Receptionists
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.Receptionists != null ?
                        View(await _context.Receptionists.ToListAsync()) :
                        Problem("Entity set 'VitalitydbContext.Receptionists'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: Receptionists/Create
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

        // POST: Receptionists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceptionistId,ReceptionistName,ReceptionistContactNo,ReceptionistEmail,ReceptionistPwd")] Receptionist receptionist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receptionist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receptionist);
        }

        // GET: Receptionists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Receptionists == null)
            {
                return NotFound();
            }

            var receptionist = await _context.Receptionists.FindAsync(id);
            if (receptionist == null)
            {
                return NotFound();
            }
            return View(receptionist);
        }

        // POST: Receptionists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceptionistId,ReceptionistName,ReceptionistContactNo,ReceptionistEmail,ReceptionistPwd")] Receptionist receptionist)
        {
            if (id != receptionist.ReceptionistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.Receptionists.Find(receptionist.ReceptionistId);
                    if (receptionist.ReceptionistName != null)
                    {
                        data.ReceptionistName = receptionist.ReceptionistName;
                    }
                    if (receptionist.ReceptionistContactNo != null)
                    {
                        data.ReceptionistContactNo = receptionist.ReceptionistContactNo;
                    }
                    if (receptionist.ReceptionistEmail != null)
                    {
                        data.ReceptionistEmail = receptionist.ReceptionistEmail;
                    }
                    if (receptionist.ReceptionistPwd != null)
                    {
                        data.ReceptionistPwd = receptionist.ReceptionistPwd;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptionistExists(receptionist.ReceptionistId))
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
            return View(receptionist);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.Receptionists == null)
                {
                    return Problem("Entity set 'VitalitydbContext.Receptionists'  is null.");
                }
                var receptionist = await _context.Receptionists.FindAsync(id);
                if (receptionist != null)
                {
                    _context.Receptionists.Remove(receptionist);
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

        private bool ReceptionistExists(int id)
        {
            return (_context.Receptionists?.Any(e => e.ReceptionistId == id)).GetValueOrDefault();
        }
    }
}
