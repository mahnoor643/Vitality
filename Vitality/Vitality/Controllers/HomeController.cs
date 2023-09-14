using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vitality.Models;

namespace Vitality.Controllers
{
    public class HomeController : Controller
    {
        private readonly VitalitydbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, VitalitydbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var ViewModel = new doctorFeedbackFetch {
                DoctorsRegistrations = _context.DoctorsRegistrations.Include(d => d.DoctorsCategoryNavigation).Where(x => x.Status == 3 || x.Status == 4).ToList(),
                Feedback = _context.Feedbacks.ToList().Where(x=>x.Status==1).ToList()
        };
            return View(ViewModel);
        }

        //Aboutus page
        public IActionResult aboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}