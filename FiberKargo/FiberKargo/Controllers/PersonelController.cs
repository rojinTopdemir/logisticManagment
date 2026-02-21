using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FiberKargo.Models;
using FiberKargo.Services;

namespace FiberKargo.Controllers
{
    public class PersonelController : Controller
    {
        private readonly FiberKargoContext _context = new FiberKargoContext();
        private readonly PriceCalculatorService _priceCalculator = new PriceCalculatorService();

        private bool IsPersonel()
        {
            var role = Session["Role"]?.ToString();
            return role == "Personel" || role == "Admin";
        }

        private string GetCurrentBranch()
        {
            return Session["Branch"]?.ToString();
        }

        private string GetCurrentUserName()
        {
            return Session["Name"]?.ToString();
        }

        public ActionResult Index()
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            ViewBag.Branch = GetCurrentBranch();
            ViewBag.UserName = GetCurrentUserName();
            return View();
        }

        #region Kargo Oluşturma

        [HttpGet]
        public ActionResult CreateCargo()
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            ViewBag.Branch = GetCurrentBranch();
            ViewBag.UserName = GetCurrentUserName();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCargo(Cargo cargo)
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            ViewBag.Branch = GetCurrentBranch();
            ViewBag.UserName = GetCurrentUserName();

            if (!ModelState.IsValid)
            {
                return View(cargo);
            }

            // Kargo numarası oluştur
            cargo.CargoNo = "K" + DateTime.Now.Ticks;
            cargo.CreateTime = DateTime.Now;
            cargo.Status = "Şubede";
            cargo.Handler = GetCurrentUserName();
            cargo.Origin = GetCurrentBranch();

            // Ücret hesapla
            if (cargo.Width.HasValue && cargo.Length.HasValue && cargo.Height.HasValue)
            {
                cargo.Price = _priceCalculator.CalculatePrice(
                    cargo.Origin ?? "",
                    cargo.Destination,
                    cargo.Weight,
                    cargo.Width.Value,
                    cargo.Length.Value,
                    cargo.Height.Value
                );
            }

            _context.Cargos.Add(cargo);
            _context.SaveChanges();

            TempData["Success"] = "Kargo başarıyla oluşturuldu.";
            return RedirectToAction("TrackCargos");
        }

        [HttpPost]
        public JsonResult CalculateCargoPrice(string destination, double weight, double width, double length, double height)
        {
            var origin = GetCurrentBranch();
            if (string.IsNullOrEmpty(origin))
            {
                return Json(new { success = false, message = "Şube bilgisi bulunamadı." });
            }

            var distance = _priceCalculator.CalculateDistance(origin, destination);
            var desi = _priceCalculator.CalculateDesi(width, length, height);
            var price = _priceCalculator.CalculatePrice(origin, destination, weight, width, length, height);

            return Json(new
            {
                success = true,
                distance = distance.ToString("F1"),
                desi = desi.ToString("F2"),
                price = price.ToString("F2")
            });
        }

        #endregion

        #region Kargo Takip

        public ActionResult TrackCargos()
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            ViewBag.Branch = GetCurrentBranch();
            ViewBag.UserName = GetCurrentUserName();

            var branch = GetCurrentBranch();
            var cargos = _context.Cargos
                .Where(c => c.Origin == branch)
                .OrderByDescending(c => c.CreateTime)
                .ToList();

            return View(cargos);
        }

        [HttpGet]
        public ActionResult EditCargo(int id)
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            var cargo = _context.Cargos.Find(id);
            if (cargo == null) return HttpNotFound();

            ViewBag.Branch = GetCurrentBranch();
            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCargo(int Id, string ReceiverName, string ReceiverTel, string Content, string Weight)
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            var existingCargo = _context.Cargos.Find(Id);
            if (existingCargo == null) return HttpNotFound();

            // Sadece formda gönderilen alanları güncelle
            existingCargo.ReceiverName = ReceiverName;
            existingCargo.ReceiverTel = ReceiverTel;
            existingCargo.Content = Content;
            
            // Ondalık ayracı sorununu çözmek için manuel parse
            var weightStr = Weight.Replace(",", ".");
            if (double.TryParse(weightStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double weightValue))
            {
                existingCargo.Weight = weightValue;
            }

            // Fiyatı yeniden hesapla
            if (existingCargo.Width.HasValue && existingCargo.Length.HasValue && existingCargo.Height.HasValue)
            {
                existingCargo.Price = _priceCalculator.CalculatePrice(
                    existingCargo.Origin ?? "",
                    existingCargo.Destination,
                    existingCargo.Weight,
                    existingCargo.Width.Value,
                    existingCargo.Length.Value,
                    existingCargo.Height.Value
                );
            }

            _context.SaveChanges();

            TempData["Success"] = "Kargo güncellendi.";
            return RedirectToAction("TrackCargos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCargo(int id)
        {
            if (!IsPersonel()) return RedirectToAction("Login", "Account");

            var cargo = _context.Cargos.Find(id);
            if (cargo == null) return HttpNotFound();

            _context.Cargos.Remove(cargo);
            _context.SaveChanges();

            TempData["Success"] = "Kargo silindi.";
            return RedirectToAction("TrackCargos");
        }

        [HttpPost]
        public JsonResult SendToDistribution(int id)
        {
            if (!IsPersonel()) return Json(new { success = false });

            var cargo = _context.Cargos.Find(id);
            if (cargo == null) return Json(new { success = false });

            cargo.Status = "Dağıtımda";
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeliverCargo(int id)
        {
            if (!IsPersonel()) return Json(new { success = false });

            var cargo = _context.Cargos.Find(id);
            if (cargo == null) return Json(new { success = false });

            cargo.Status = "Teslim Edildi";
            cargo.DeliveryTime = DateTime.Now;
            _context.SaveChanges();

            return Json(new { success = true });
        }

        #endregion

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
