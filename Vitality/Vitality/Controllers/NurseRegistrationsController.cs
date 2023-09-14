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
    public class NurseRegistrationsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly VitalitydbContext _context;
        List<string> sessionInfo = new List<string>();

        public NurseRegistrationsController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: NurseRegistrations
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.NurseRegistrations.Include(n => n.DegreeNavigation);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }


        // GET: NurseRegistrations/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }

        }

        // POST: NurseRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseRegistrationsId,NurseName,NurseEmail,NursePhoneNo,Degree,NurseImg,Status")] NurseRegistration nurseRegistration, IFormFile file)
        {
            var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("", "File Extension Is Invalid");
                return View(nurseRegistration);
            }
            else if (file.Length > 2097125)
            {
                var filesize = 1024;
                ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                return View(nurseRegistration);
            }
            else
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string imagesFolderPath = Path.Combine(webRootPath, "NurseImgs");
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }
                string imageName = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(imagesFolderPath, imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                nurseRegistration.NurseImg = "../../NurseImgs/" + imageName;
                nurseRegistration.Status = 0;
                _context.Add(nurseRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        // GET: NurseRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NurseRegistrations == null)
            {
                return NotFound();
            }

            var nurseRegistration = await _context.NurseRegistrations.FindAsync(id);
            if (nurseRegistration == null)
            {
                return NotFound();
            }
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        // POST: NurseRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NurseRegistrationsId,NurseName,NurseEmail,NursePhoneNo,Degree,NurseImg,Status")] NurseRegistration nurseRegistration, IFormFile file)
        {
            if (id != nurseRegistration.NurseRegistrationsId)
            {
                return NotFound();
            }

            var existingNurse = _context.NurseRegistrations.Find(nurseRegistration.NurseRegistrationsId);
            if (file != null)
            {
                var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
                if (!supportedTypes.Contains(fileExt))
                {
                    ModelState.AddModelError("", "File Extension Is Invalid");
                    return View(nurseRegistration);
                }
                else if (file.Length > 2097125)
                {
                    var filesize = 1024;
                    ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                    return View(nurseRegistration);
                }
                else
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string imagesFolderPath = Path.Combine(webRootPath, "NurseImgs");
                    if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }
                    string imageName = Path.GetFileName(file.FileName);
                    string imagePath = Path.Combine(imagesFolderPath, imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    existingNurse.NurseImg = "../../NurseImgs/" + imageName;
                }
            }
            if (nurseRegistration.NurseName != null)
            {
                existingNurse.NurseName = nurseRegistration.NurseName;
            }
            if (nurseRegistration.NurseEmail != null)
            {
                existingNurse.NurseEmail = nurseRegistration.NurseEmail;
            }
            if (nurseRegistration.NursePhoneNo != null)
            {
                existingNurse.NursePhoneNo = nurseRegistration.NursePhoneNo;
            }
            if (nurseRegistration.Degree != null)
            {
                existingNurse.Degree = nurseRegistration.Degree;
            }
            if (nurseRegistration.Status != null)
            {
                existingNurse.Status = nurseRegistration.Status;
            }

            _context.Update(nurseRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (_context.NurseRegistrations == null)
                {
                    return Problem("Entity set 'VitalitydbContext.NurseRegistrations'  is null.");
                }
                var nurseRegistration = await _context.NurseRegistrations.FindAsync(id);
                if (nurseRegistration != null)
                {
                    _context.NurseRegistrations.Remove(nurseRegistration);
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

        //Deactivating Doctors from admin
        public IActionResult Deactive(int id)
        {

            var nurseDeactive = _context.NurseRegistrations.FirstOrDefault(c => c.NurseRegistrationsId == id);

            if (nurseDeactive != null)
            {
                nurseDeactive.Status = 0;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Activating Doctors fromm admin

        public IActionResult Active(int id)
        {

            var nurseActive = _context.NurseRegistrations.FirstOrDefault(c => c.NurseRegistrationsId == id);

            if (nurseActive != null)
            {
                nurseActive.Status = 1;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //add Nurse from nurse dashboard
        public IActionResult registerNurse()
        {
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1");
            return View();
        }

        // POST: NurseRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerNurse([Bind("NurseRegistrationsId,NurseName,NurseEmail,NursePhoneNo,Degree,NurseImg,Status")] NurseRegistration nurseRegistration, IFormFile file)
        {

            var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("", "File Extension Is Invalid");
                return View(nurseRegistration);
            }
            else if (file.Length > 2097125)
            {
                var filesize = 1024;
                ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                return View(nurseRegistration);
            }
            else
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string imagesFolderPath = Path.Combine(webRootPath, "NurseImgs");
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }
                string imageName = Path.GetFileName(file.FileName);
                string imagePath = Path.Combine(imagesFolderPath, imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                nurseRegistration.NurseImg = "../../NurseImgs/" + imageName;
                nurseRegistration.Status = 0;
                _context.Add(nurseRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        //Edit from nurse dashbooard
        public async Task<IActionResult> nurseEdit(int? id)
        {
            if (id == null || _context.NurseRegistrations == null)
            {
                return NotFound();
            }

            var nurseRegistration = await _context.NurseRegistrations.FindAsync(id);
            if (nurseRegistration == null)
            {
                return NotFound();
            }
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        // POST: NurseRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> nurseEdit(int id, [Bind("NurseRegistrationsId,NurseName,NurseEmail,NursePhoneNo,Degree,NurseImg,Status")] NurseRegistration nurseRegistration, IFormFile file)
        {
            if (id != nurseRegistration.NurseRegistrationsId)
            {
                return NotFound();
            }

            var existingNurse = _context.NurseRegistrations.Find(nurseRegistration.NurseRegistrationsId);
            if (file != null)
            {
                var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
                if (!supportedTypes.Contains(fileExt))
                {
                    ModelState.AddModelError("", "File Extension Is Invalid");
                    return View(nurseRegistration);
                }
                else if (file.Length > 2097125)
                {
                    var filesize = 1024;
                    ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                    return View(nurseRegistration);
                }
                else
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string imagesFolderPath = Path.Combine(webRootPath, "NurseImgs");
                    if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }
                    string imageName = Path.GetFileName(file.FileName);
                    string imagePath = Path.Combine(imagesFolderPath, imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    existingNurse.NurseImg = "../../NurseImgs/" + imageName;
                }
            }
            if (nurseRegistration.NurseName != null)
            {
                existingNurse.NurseName = nurseRegistration.NurseName;
            }
            if (nurseRegistration.NurseEmail != null)
            {
                existingNurse.NurseEmail = nurseRegistration.NurseEmail;
            }
            if (nurseRegistration.NursePhoneNo != null)
            {
                existingNurse.NursePhoneNo = nurseRegistration.NursePhoneNo;
            }
            if (nurseRegistration.Degree != null)
            {
                existingNurse.Degree = nurseRegistration.Degree;
            }
            if (nurseRegistration.Status != null)
            {
                existingNurse.Status = nurseRegistration.Status;
            }

            _context.Update(nurseRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewData["Degree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", nurseRegistration.Degree);
            return View(nurseRegistration);
        }

        //Login page
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(NurseRegistration login)
        {
            var login_info = _context.NurseRegistrations.Where(x => x.NurseEmail == login.NurseEmail && x.NursePhoneNo == login.NursePhoneNo)
                .FirstOrDefault();
            if (login_info != null)
            {
                HttpContext.Session.SetInt32(SessionVariables.SessionNurseID, login_info.NurseRegistrationsId);
                var nurseId = HttpContext.Session.GetInt32(SessionVariables.SessionNurseID);
                string nurse = nurseId?.ToString() ?? string.Empty;
                sessionInfo.Add(nurse);
                return RedirectToAction("Create", "NurseRegistrations");
            }
            else
            {
                return View();
            }
        }

        private bool NurseRegistrationExists(int id)
        {
            return (_context.NurseRegistrations?.Any(e => e.NurseRegistrationsId == id)).GetValueOrDefault();
        }
    }
}
