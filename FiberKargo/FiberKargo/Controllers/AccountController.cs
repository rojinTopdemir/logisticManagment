using FiberKargo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FiberKargo.Services;

namespace FiberKargo.Controllers
{
    public class AccountController : Controller
    {
        private readonly FiberKargoContext _context = new FiberKargoContext();
        private readonly CaptchaService _captchaService = new CaptchaService();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                var role = Session["Role"]?.ToString();
                return role == "Admin" ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Personel");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Captcha kontrolü
            var storedCaptcha = Session["Captcha"]?.ToString();
            if (string.IsNullOrEmpty(storedCaptcha) || !_captchaService.ValidateCaptcha(model.Captcha, storedCaptcha))
            {
                ModelState.AddModelError("Captcha", "Güvenlik kodu hatalı! (Büyük/Küçük harfe dikkat edin)");
                return View(model);
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            // Session'a kullanıcı bilgilerini kaydet
            Session["UserId"] = user.Id;
            Session["Username"] = user.Username;
            Session["Name"] = user.Name;
            Session["Role"] = user.Role;
            Session["Branch"] = user.Branch;

            // Yönlendirme
            return user.Role == "Admin"
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "Personel");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult RefreshCaptcha()
        {
            var captcha = _captchaService.GenerateCaptcha();
            Session["Captcha"] = captcha;
            return Json(new { captcha }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}