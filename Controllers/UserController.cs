using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;

namespace School.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        public UserController(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        //LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Login(User user)
        {
            var existing = dbcontext.users.FirstOrDefault(kamal => kamal.Email == user.Email);
            if (existing != null)
            {
                if (existing.Email == user.Email && existing.Password == user.Password)
                {
                    return RedirectToAction("Index", "Student", dbcontext.students);
                }
                else
                {
                    ViewBag.error = "data are incorrect";
                    return View(user);
                }
            }
            else
            {
                ViewBag.error = "user dose not exist";
                return View(user);
            }
        }
        [HttpGet]
        //GET REGISTER
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        //POST REGISTER
        [HttpPost]
        public IActionResult Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            dbcontext.users.Add(user);
            dbcontext.SaveChanges();
            return View("Login");
        }

    }
}
