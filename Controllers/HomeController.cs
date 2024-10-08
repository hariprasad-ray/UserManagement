using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManagementContext _context;

        public HomeController(ILogger<HomeController> logger, UserManagementContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult List()
        {
            int totalCount = 0, activeCount = 0, inActiveCount = 0;
            var data = new List<UserDetail>();

            try
            {
                data = _context.UserDetails.ToList();

                if (data != null)
                {
                    totalCount = data.Count;
                    activeCount = data.Where(p => p.IsActive == true).Count();
                    inActiveCount = data.Where(p => p.IsActive == false).Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            ViewBag.TotalUserCount = totalCount;
            ViewBag.ActiveUserCount = activeCount;
            ViewBag.InActiveUserCount = inActiveCount;

            return View(data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
