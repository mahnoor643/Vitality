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
    public class ContactUsController : Controller
    {
        private readonly VitalitydbContext _context;

        public ContactUsController(VitalitydbContext context)
        {
            _context = context;
        }

        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.ContactUs != null ?
                        View(await _context.ContactUs.ToListAsync()) :
                        Problem("Entity set 'VitalitydbContext.ContactUs'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }



        // GET: ContactUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,UserName,UserEmail,ContactSubject,ContactMsg")] ContactU contactU)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactU);
                await _context.SaveChangesAsync();
                TempData["ErrorMessage"] = "Your message is submitted!";
                return RedirectToAction("Index", "Home");
            }
            return View(contactU);
        }


        //Delete functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.ContactUs == null)
                {
                    return Problem("Entity set 'VitalitydbContext.ContactUs'  is null.");
                }
                var contactU = await _context.ContactUs.FindAsync(id);
                if (contactU != null)
                {
                    _context.ContactUs.Remove(contactU);
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

        private bool ContactUExists(int id)
        {
            return (_context.ContactUs?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
