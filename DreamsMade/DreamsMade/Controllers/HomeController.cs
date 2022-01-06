using DreamsMade.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

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
        [Authorize]
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
                //Encryption encrypter = new Encryption();
                //user.password = encrypter.encrypt(user.password);

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
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                Context context = new Context();
                //Encryption encrypter = new Encryption();
                //user.password = encrypter.decrypt(user.password);
                var userLogin = (from User u in context.Users select u).Where(n => n.name == user.name && n.password == user.password).FirstOrDefault();


                if (userLogin != null)
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.name),
                        new Claim(ClaimTypes.Role, "user"),
                    };

                    var identidade = new ClaimsIdentity(claims, "Login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(identidade);
                    var regrasAutenticacao = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddHours(4),
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, regrasAutenticacao
                        );

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Ocorreu um problema ao autenticar: " + ex.Message;
            }
            return View();
        }

        //---------------------------------------------------------------------------------------------------------------------------

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
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