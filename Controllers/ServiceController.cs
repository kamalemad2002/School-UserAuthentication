using Microsoft.AspNetCore.Mvc;
using School.Helpers;
using School.Services;
using School.Storage;

namespace School.Controllers
{
    public class ServiceController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public IActionResult Encrypt()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                ViewBag.Error = "Please enter text to encrypt.";
                return View();
            }

            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Accoudnt");
            }

            string cipherText = EncryptionServiceRSA.RSAEncrypt(plainText, false, email);
            if (cipherText == null)
            {
                ViewBag.Error = "Encryption failed.";
                return View();
            }

            ViewBag.PlainText = plainText;
            ViewBag.CipherText = cipherText;
            return View();
        }
        
        [HttpGet]
        public IActionResult Decrypt()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User");
            }

            var decryptedTexts = EncryptionServiceRSA.RSADecrypt(email, false);
            ViewBag.DecryptedTexts = decryptedTexts;
            return View();
        }
        [HttpGet]
        public IActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Reset(string Password)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Please enter a valid email.";
                return View();
            }

            // Call your service to reset the password
            School.Services.PassServices.Reset(email,Password);
            return RedirectToAction("Login", "User"); 
        }

        [HttpGet]
        public IActionResult Change()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Change(string currentPassword, string newPassword)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            try
            {
                School.Services.PassServices.Change(email, currentPassword, newPassword);
                return RedirectToAction("Index", "Service");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message; 
                return View();
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            var users = FileManager.LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ViewBag.Error = "Email not found.";
                return View();
            }
            HttpContext.Session.SetString("Email", Email);
            return RedirectToAction("Reset");
        }


    }
}
