using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class LabTestsController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public LabTestsController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: LabTests
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                return _context.LabTests != null ?
                          View(await _context.LabTests.ToListAsync()) :
                          Problem("Entity set 'VitalitydbContext.LabTests'  is null.");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: LabTests/Create
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

        // POST: LabTests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabTestId,LabTest1,LabTestAmount")] LabTest labTest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labTest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labTest);
        }

        // GET: LabTests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabTests == null)
            {
                return NotFound();
            }

            var labTest = await _context.LabTests.FindAsync(id);
            if (labTest == null)
            {
                return NotFound();
            }
            return View(labTest);
        }

        // POST: LabTests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabTestId,LabTest1,LabTestAmount")] LabTest labTest)
        {
            if (id != labTest.LabTestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.LabTests.Find(labTest.LabTestId);
                    if (labTest.LabTest1 != null)
                    {
                        data.LabTest1 = labTest.LabTest1;
                    }
                    if (labTest.LabTestAmount != null)
                    {
                        data.LabTestAmount = labTest.LabTestAmount;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabTestExists(labTest.LabTestId))
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
            return View(labTest);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.LabTests == null)
                {
                    return Problem("Entity set 'VitalitydbContext.LabTests'  is null.");
                }
                var labTest = await _context.LabTests.FindAsync(id);
                if (labTest != null)
                {
                    _context.LabTests.Remove(labTest);
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

        private bool LabTestExists(int id)
        {
            return (_context.LabTests?.Any(e => e.LabTestId == id)).GetValueOrDefault();
        }
    }
}
