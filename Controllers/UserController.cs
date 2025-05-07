using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using School.Models;
using School.Storage;

namespace School.Controllers
{
    public class UserController : Controller
    {

        public UserController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        //LOGIN mE
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var users = FileManager.LoadUsers();
                var user = users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null || user.Password != Helpers.HashedPasswordSHA256.HashPassword(model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Please check your email and password.");
                    ViewBag.error = "Please enter a valid email and password.";
                    return View("Login");
                }
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectToAction("Index", "Service");
            }
            return View();
        }




        //GET REGISTER
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        //POST REGISTER
        [HttpPost]
        public IActionResult Signup(RegisterModel user)
        {
            var users = FileManager.LoadUsers();
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if (user.Password.Length < 8)
            {
                ViewBag.error = "Password must be at least 8 characters long!!";
                return View(user);
            }

            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                ViewBag.error = "Email is already exits!!";
                return View(user);
            }
            var hashed = School.Helpers.HashedPasswordSHA256.HashPassword(user.Password);
            FileManager.SaveUser(new RegisterModel { Email = user.Email, Password = hashed });
            return View("Login");
        }



        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

    }
}
