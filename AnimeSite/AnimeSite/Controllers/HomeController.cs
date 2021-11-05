using AnimeSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace AnimeSite.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        //////////////////////////////////////////
        public IActionResult Authorization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(LogUser a)
        {

            User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == a.Login && predicate.Password == a.Password);
            if (user != null)
            {

                return RedirectToAction("Index");
            }
            return NotFound();
        }
        ///////////////////////////////////////////
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
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
