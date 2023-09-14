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
    public class DoctorCategoriesController : Controller
    {
        private readonly VitalitydbContext _context;

        public DoctorCategoriesController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: DoctorCategories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {

                return _context.DoctorCategories != null ?
                        View(await _context.DoctorCategories.ToListAsync()) :
                        Problem("Entity set 'VitalitydbContext.DoctorCategories'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // GET: DoctorCategories/Create
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

        // POST: DoctorCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorCategoryId,DoctorCategory1")] DoctorCategory doctorCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctorCategory);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            try
            {
                var doctorCategory = await _context.DoctorCategories.FindAsync(id);
                if (doctorCategory != null)
                {
                    _context.DoctorCategories.Remove(doctorCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle the case where the doctorCategory with the provided ID was not found.
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You can't delete cause this data is used!";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool DoctorCategoryExists(int id)
        {
            return (_context.DoctorCategories?.Any(e => e.DoctorCategoryId == id)).GetValueOrDefault();
        }
    }
}
