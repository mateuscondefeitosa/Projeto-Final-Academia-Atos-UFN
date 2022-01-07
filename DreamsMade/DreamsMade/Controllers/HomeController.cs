using DreamsMade.Crypto;
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
        private readonly Context _dbContext;

        private readonly ICrypto _crypto;

        UserResponse _userresponse;

        public HomeController(ICrypto crypto, Context context, UserResponse userresponse)
        {
            _crypto = crypto;
            _dbContext = context;
            _userresponse = userresponse;
        }

        public IActionResult Index()
        {
            return View();
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult Dreams()
        {
            List<Post> posts = (from Post p in _dbContext.Posts select p).Include(e => e.user).ToList<Post>();

            return View(posts);
        }


        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult MyPage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            List<Post> posts = (from Post p in _dbContext.Posts select p).Include(e => e.user).Where(e => e.user.id == _userresponse.id).ToList<Post>();

            return View(posts);
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult NewPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewPost(Post post)
        {
            try
            {
                var pesquisaUser = (from User u in _dbContext.Users select u).Where(u => u.id == _userresponse.id).FirstOrDefault<User>();
                post.user = pesquisaUser;
                
                await _dbContext.Posts.AddAsync(post);
                await _dbContext.SaveChangesAsync(); 

                return RedirectToAction("MyPage");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

                user.password = _crypto.Encrypt(user.password);

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();


                return RedirectToAction("MyPage");
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
                user.password = _crypto.Encrypt(user.password);
                var userLogin = (from User u in _dbContext.Users select u).Where(n => n.name == user.name && n.password == user.password).FirstOrDefault();

                if (userLogin != null)
                {
                    _userresponse.id = userLogin.id;
                    //_userresponse.name = userLogin.name;

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

                    return RedirectToAction("MyPage");
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
            _userresponse.id = null;
            //_userresponse.name = String.Empty;

            await HttpContext.SignOutAsync();
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