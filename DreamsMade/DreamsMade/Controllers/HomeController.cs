using DreamsMade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DreamsMade.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}


        public IActionResult Index()
        {
            return View();
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult Dreams()
        {
            Context context = new Context();

            List<Post> posts = (from Post p in context.Posts select p).Include(e => e.user).ToList<Post>();

            return View(posts);
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
                Encryption encrypter = new Encryption();

                user.password = encrypter.encrypt(user.password);

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
            try
            {
                //if (user != null)
                //{
                //    HttpContext.Session.SetInt32("Id", user.Id);
                //    HttpContext.Session.SetString("Login", user.Login);
                //    HttpContext.Session.SetString("Senha", user.Senha);
                //    return RedirectToAction(nameof(MyPage));
                //}
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        //---------------------------------------------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}