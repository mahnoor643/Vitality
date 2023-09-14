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
    public class DoctorsRegistrationsController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<string> sessionInfo = new List<string>();

        public DoctorsRegistrationsController(VitalitydbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: DoctorsRegistrations
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.DoctorsRegistrations.Include(d => d.DoctorsCategoryNavigation).Include(d => d.DoctorsDegreeNavigation).Where(x => x.Status == 1 || x.Status == 0 || x.Status == 3);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        //Top Doctors
        public async Task<IActionResult> topDoctors()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                var vitalitydbContext = _context.DoctorsRegistrations.Include(d => d.DoctorsCategoryNavigation).Include(d => d.DoctorsDegreeNavigation).Where(x => x.Status == 3);
                return View(await vitalitydbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        //Doctors Fetch
        public async Task<IActionResult> fetchDoctor(string searchresult)
        {

            var vitalitydbContext = _context.DoctorsRegistrations
    .Include(d => d.DoctorsCategoryNavigation)
    .Include(d => d.DoctorsDegreeNavigation)
    .Where(d => d.Status == 1 || d.Status == 3 || d.Status == 4);

            if (!string.IsNullOrEmpty(searchresult))
            {
                vitalitydbContext = vitalitydbContext
                    .Where(d => d.DoctorsCategoryNavigation.DoctorCategory1.Contains(searchresult));
            }

            return View(await vitalitydbContext.ToListAsync());


        }

        //Dashboard
        public async Task<IActionResult> dashboard()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) != null)
            {
                var doctorID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID));
                var Variable_Name = DateTime.Now.Date; // Use DateTime type
                var ViewModel = new doctorDashboard
                {
                    DoctorsAppointments = _context.DoctorsAppointments.Where(x => x.DoctorAppointment == doctorID).Include(d => d.DoctorAppointmentNavigation).Include(d => d.DoctorAppointmentTimeNavigation).Include(d => d.Patients).Where(x => x.DoctorAppointmentDate == Variable_Name).ToList(),
                    PatientsRegistrationsCount = _context.PatientsRegistrations.Count(),
                    DoctorsAppointmentsCount = _context.DoctorsAppointments.Where(x => x.DoctorAppointment == doctorID).Count(),
                    TodaysAppointment = _context.DoctorsAppointments.Where(x => x.DoctorAppointment == doctorID).Where(x => x.DoctorAppointmentDate == Variable_Name).Count()
                };
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "DoctorsRegistrations");
            }
        }

        public async Task<IActionResult> appointmentFetch()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID) != null)
            {
                var doctorID = Convert.ToInt32(HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID));
                var appointment = _context.DoctorsAppointments.Include(d => d.DoctorAppointmentNavigation).Include(d => d.DoctorAppointmentTimeNavigation).Include(d => d.Patients).Where(x => x.DoctorAppointment == doctorID);
                return View(await appointment.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "DoctorsRegistrations");
            }
        }



        // GET: DoctorsRegistrations/Create
        public IActionResult Create()
        {
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1");
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1");
            return View();
        }

        // POST: DoctorsRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorsId,DoctorsName,DoctorsDegree,DoctorsCategory,DoctorsPhoneNo,Status,DoctorsEmail,DoctorsImg,DoctorsPwd")] DoctorsRegistration doctorsRegistration, IFormFile file)
        {
            var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("", "File Extension Is Invalid");
                return View(doctorsRegistration);
            }
            else if (file.Length > 2097125)
            {
                var filesize = 1024;
                ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                return View(doctorsRegistration);
            }
            else
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string imagesFolderPath = Path.Combine(webRootPath, "image");
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
                doctorsRegistration.DoctorsImg = "../../image/" + imageName;
                _context.Add(doctorsRegistration);
                _context.SaveChanges();
                TempData["ErrorMessage"] = "You have registered your account successfully, Now Login with your new Account..";
                return RedirectToAction("Login");
            }
            /*if (ModelState.IsValid)
            {
                _context.Add(doctorsRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
            return View(doctorsRegistration);
        }

        // GET: DoctorsRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DoctorsRegistrations == null)
            {
                return NotFound();
            }

            var doctorsRegistration = await _context.DoctorsRegistrations.FindAsync(id);
            if (doctorsRegistration == null)
            {
                return NotFound();
            }
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
            return View(doctorsRegistration);
        }

        // POST: DoctorsRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorsId,DoctorsName,DoctorsDegree,DoctorsCategory,DoctorsPhoneNo,Status,DoctorsEmail,DoctorsImg,DoctorsPwd")] DoctorsRegistration doctorsRegistration, IFormFile file)
        {
            if (id != doctorsRegistration.DoctorsId)
            {
                return NotFound();
            }
            else
            {
                var existingDoctor = _context.DoctorsRegistrations.Find(doctorsRegistration.DoctorsId);


                if (file != null)
                {
                    var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
                    var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("", "File Extension Is Invalid");
                        return View(doctorsRegistration);
                    }
                    else if (file.Length > 2097125)
                    {
                        var filesize = 1024;
                        ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                        return View(doctorsRegistration);
                    }
                    else
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string imagesFolderPath = Path.Combine(webRootPath, "image");
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
                        existingDoctor.DoctorsImg = "../../image/" + imageName;
                    }
                }
                if (doctorsRegistration.DoctorsName != null)
                {
                    existingDoctor.DoctorsName = doctorsRegistration.DoctorsName;
                }
                if (doctorsRegistration.DoctorsDegree != null)
                {
                    existingDoctor.DoctorsDegree = doctorsRegistration.DoctorsDegree;
                }
                if (doctorsRegistration.DoctorsPwd != null)
                {
                    existingDoctor.DoctorsPwd = doctorsRegistration.DoctorsPwd;
                }
                if (doctorsRegistration.DoctorsCategory != null)
                {
                    existingDoctor.DoctorsCategory = doctorsRegistration.DoctorsCategory;
                }
                if (doctorsRegistration.DoctorsPhoneNo != null)
                {
                    existingDoctor.DoctorsPhoneNo = doctorsRegistration.DoctorsPhoneNo;
                }
                if (doctorsRegistration.DoctorsEmail != null)
                {
                    existingDoctor.DoctorsEmail = doctorsRegistration.DoctorsEmail;
                }
            }
            _context.SaveChanges();
            TempData["ErrorMessage"] = "Your data is modified.";
            return RedirectToAction("dashboard");
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
            return View(doctorsRegistration);
        }

        //Delete Functionality
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                var doctorsRegistration = await _context.DoctorsRegistrations.FindAsync(id);
                if (doctorsRegistration != null)
                {
                    _context.DoctorsRegistrations.Remove(doctorsRegistration);
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

        private bool DoctorsRegistrationExists(int id)
        {
            return (_context.DoctorsRegistrations?.Any(e => e.DoctorsId == id)).GetValueOrDefault();
        }

        //Deactivating Doctors from admin
        public IActionResult Deactive(int id)
        {

            var doctorDeactive = _context.DoctorsRegistrations.FirstOrDefault(c => c.DoctorsId == id);

            if (doctorDeactive != null)
            {
                doctorDeactive.Status = 0;
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

            var doctorActive = _context.DoctorsRegistrations.FirstOrDefault(c => c.DoctorsId == id);

            if (doctorActive != null)
            {
                doctorActive.Status = 1;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Activating Doctors as Best Doctor

        public IActionResult bestDoctor(int id)
        {

            var doctorActive = _context.DoctorsRegistrations.FirstOrDefault(c => c.DoctorsId == id);

            if (doctorActive != null)
            {
                doctorActive.Status = 3;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(topDoctors));
        }

        //BestDoctot titleRemoving
        //Activating Doctors fromm admin

        public IActionResult removeBestDoctor(int id)
        {

            var doctorActive = _context.DoctorsRegistrations.FirstOrDefault(c => c.DoctorsId == id);

            if (doctorActive != null)
            {
                doctorActive.Status = 1;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: DoctorsRegistrations/addDoctor
        public IActionResult addDoctor()
        {
            if (HttpContext.Session.GetInt32(SessionVariables.SessionAdminID) != null)
            {
                ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1");
                ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }

        // POST: DoctorsRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addDoctor([Bind("DoctorsId,DoctorsName,DoctorsDegree,DoctorsCategory,DoctorsPhoneNo,Status,DoctorsEmail,DoctorsImg,DoctorsPwd")] DoctorsRegistration doctorsRegistration, IFormFile file)
        {
            var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("", "File Extension Is Invalid");
                return View(doctorsRegistration);
            }
            else if (file.Length > 2097125)
            {
                var filesize = 1024;
                ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                return View(doctorsRegistration);
            }
            else
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string imagesFolderPath = Path.Combine(webRootPath, "image");
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
                doctorsRegistration.DoctorsImg = "../../image/" + imageName;
                _context.Add(doctorsRegistration);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
            return View(doctorsRegistration);
        }

        // DoctorEdit
        public async Task<IActionResult> doctorEdit(int? id)
        {
            if (id == null || _context.DoctorsRegistrations == null)
            {
                return NotFound();
            }

            var doctorsRegistration = await _context.DoctorsRegistrations.FindAsync(id);
            if (doctorsRegistration == null)
            {
                return NotFound();
            }
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
            return View(doctorsRegistration);
        }

        // POST: DoctorsRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> doctorEdit(int id, [Bind("DoctorsId,DoctorsName,DoctorsDegree,DoctorsCategory,DoctorsPhoneNo,Status,DoctorsEmail,DoctorsImg,DoctorsPwd")] DoctorsRegistration doctorsRegistration, IFormFile file)
        {
            if (id != doctorsRegistration.DoctorsId)
            {
                return NotFound();
            }
            else
            {
                var existingDoctor = _context.DoctorsRegistrations.Find(doctorsRegistration.DoctorsId);


                if (file != null)
                {
                    var supportedTypes = new[] { "png", "jpeg", "jpg", "gif", "svg", "jfif" };
                    var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("", "File Extension Is Invalid");
                        return View(doctorsRegistration);
                    }
                    else if (file.Length > 2097125)
                    {
                        var filesize = 1024;
                        ModelState.AddModelError("", "File size should be up to " + filesize + "KB");
                        return View(doctorsRegistration);
                    }
                    else
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string imagesFolderPath = Path.Combine(webRootPath, "image");
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
                        existingDoctor.DoctorsImg = "../../image/" + imageName;
                    }
                }
                if (doctorsRegistration.DoctorsName != null)
                {
                    existingDoctor.DoctorsName = doctorsRegistration.DoctorsName;
                }
                if (doctorsRegistration.DoctorsPwd != null)
                {
                    existingDoctor.DoctorsPwd = doctorsRegistration.DoctorsPwd;
                }
                if (doctorsRegistration.DoctorsDegree != null)
                {
                    existingDoctor.DoctorsDegree = doctorsRegistration.DoctorsDegree;
                }
                if (doctorsRegistration.DoctorsCategory != null)
                {
                    existingDoctor.DoctorsCategory = doctorsRegistration.DoctorsCategory;
                }
                if (doctorsRegistration.DoctorsPhoneNo != null)
                {
                    existingDoctor.DoctorsPhoneNo = doctorsRegistration.DoctorsPhoneNo;
                }
                if (doctorsRegistration.DoctorsEmail != null)
                {
                    existingDoctor.DoctorsEmail = doctorsRegistration.DoctorsEmail;
                }
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
            ViewData["DoctorsCategory"] = new SelectList(_context.DoctorCategories, "DoctorCategoryId", "DoctorCategory1", doctorsRegistration.DoctorsCategory);
            ViewData["DoctorsDegree"] = new SelectList(_context.Degrees, "DegreeId", "Degree1", doctorsRegistration.DoctorsDegree);
        }

        //Login page
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(DoctorsRegistration login)
        {
            var login_info = _context.DoctorsRegistrations.Where(x => x.DoctorsEmail == login.DoctorsEmail && x.DoctorsPwd == login.DoctorsPwd)
                .FirstOrDefault();
            if (login_info != null)
            {
                HttpContext.Session.SetInt32(SessionVariables.SessionDoctorsID, login_info.DoctorsId);
                var doctorId = HttpContext.Session.GetInt32(SessionVariables.SessionDoctorsID);
                string doctor = doctorId?.ToString() ?? string.Empty;
                sessionInfo.Add(doctor);
                return RedirectToAction("dashboard", "DoctorsRegistrations");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToAction("Login");
            }
        }

        //Logout session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionVariables.SessionDoctorsID); // Remove specific item from the session

            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }
    }
}
