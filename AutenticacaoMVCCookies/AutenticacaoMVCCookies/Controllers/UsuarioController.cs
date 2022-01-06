using AutenticacaoMVCCookies.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutenticacaoMVCCookies.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPage(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // CONSULTA NO BANCO AQUI
                    if (usuario.Login == "fabricio" && usuario.Senha== "123")
                    {
                        //-----------------

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Login),
                            new Claim(ClaimTypes.Role, "admin"),
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

                        //-----------------
                        return RedirectToAction("UserPage");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Ocorreu um problema ao autenticar: " + ex.Message;
            }

            return View();
        }

        [Authorize]
        public IActionResult UserPage()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            // HttpContext.Session.Clear();    
            return RedirectToAction("Index");
        }
    }
}
