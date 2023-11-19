using FAP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FAP.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private ProjectPRNContext _context;

		public HomeController(ILogger<HomeController> logger, ProjectPRNContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var userRole = HttpContext.Session.GetString("UserRole");

                ViewBag.UserId = userId;
                ViewBag.UserRole = userRole;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Login(string email)
        {
            var user = _context.Accounts.SingleOrDefault(a => a.Email == email);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Code);
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Invalid email";
            return View();
        }

        [Authorize]
        public IActionResult LoginGoogle()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
			HttpContext.Session.Remove("UserRole");
			return Redirect("Login");
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