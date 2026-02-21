using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FiberKargo.Models;
using FiberKargo.Services;

namespace FiberKargo.Controllers
{
    public class HomeController : Controller
    {
        private readonly FiberKargoContext _context = new FiberKargoContext();
        private readonly PriceCalculatorService _priceCalculator = new PriceCalculatorService();
        private readonly CaptchaService _captchaService = new CaptchaService();

        public ActionResult Index()
        {
            ViewBag.Branches = _context.Branches.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult TrackCargo(string phoneNumber)
        {
            ViewBag.Branches = _context.Branches.ToList();

            if (string.IsNullOrEmpty(phoneNumber) || !System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^5\d{9}$"))
            {
                ViewBag.TrackError = "Telefon 5 ile başlamalı ve 10 haneli olmalı";
                return View("Index");
            }

            var results = _context.Cargos
                .Where(c => c.SenderTel == phoneNumber || c.ReceiverTel == phoneNumber)
                .ToList();

            ViewBag.TrackResults = results;
            ViewBag.SearchedPhone = phoneNumber;
            return View("Index");
        }

        [HttpPost]
        public JsonResult CalculatePrice(PriceCalculatorViewModel model)
        {
            if (model.Weight <= 0 || model.Width <= 0 || model.Length <= 0 || model.Height <= 0)
            {
                return Json(new { success = false, message = "Kilo ve boyut değerleri 0'dan büyük olmalıdır!" });
            }

            var distance = _priceCalculator.CalculateDistance(model.FromCity, model.ToCity);
            var desi = _priceCalculator.CalculateDesi(model.Width, model.Length, model.Height);
            var price = _priceCalculator.CalculatePrice(model.FromCity, model.ToCity, model.Weight, model.Width, model.Length, model.Height);

            return Json(new
            {
                success = true,
                distance = distance.ToString("F1"),
                desi = desi.ToString("F2"),
                price = price.ToString("F2")
            });
        }

        [HttpPost]
        public ActionResult SubmitFeedback(Feedback feedback)
        {
            if (string.IsNullOrEmpty(feedback.Name) || string.IsNullOrEmpty(feedback.Branch) || feedback.Rating < 1)
            {
                TempData["FeedbackError"] = "Lütfen adınızı, şubenizi ve puanınızı girin.";
                return RedirectToAction("Index");
            }

            feedback.CreateTime = System.DateTime.Now;
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            TempData["FeedbackSuccess"] = "Teşekkür ederiz! Geri bildiriminiz kaydedildi. 💖";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetCaptcha()
        {
            var captcha = _captchaService.GenerateCaptcha();
            Session["Captcha"] = captcha;
            return Json(new { captcha }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
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