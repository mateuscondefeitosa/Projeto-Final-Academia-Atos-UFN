using DreamsMade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DreamsMade.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult Dreams()
        {
            return View();
        }


        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult MyPage(int id)
        {
            Context context = new Context();
            User? autor = context.Users.Find(id);

            return View(autor);
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            try
            {
                Context context = new Context();

                context.Users.Add(user);
                context.SaveChanges();

                return RedirectToAction("MyPage", new {id = user.id});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            try
            {
                //XXXXXXXXXXXXXXXXXXXXXXXXX

                return RedirectToAction("MyPage", new { id = user.id });
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------
        
        [HttpPost]
        public IActionResult Logout(User user)
        {
            //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            return RedirectToAction("Index");
        }

        //---------------------------------------------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}