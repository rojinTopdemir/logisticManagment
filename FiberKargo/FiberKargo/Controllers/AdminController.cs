using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FiberKargo.Models
{
    public class AdminController : Controller
    {
        private readonly FiberKargoContext _context = new FiberKargoContext();

        private bool IsAdmin()
        {
            return Session["Role"]?.ToString() == "Admin";
        }

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var dashboard = new DashboardViewModel
            {
                TotalCargos = _context.Cargos.Count(),
                DeliveredCargos = _context.Cargos.Count(c => c.Status == "Teslim Edildi"),
                StaffCount = _context.Users.Count(u => u.Role == "Personel"),
                TotalFeedbacks = _context.Feedbacks.Count(),
                AverageRating = _context.Feedbacks.Any()
                    ? _context.Feedbacks.Average(f => f.Rating)
                    : 0
            };

            return View(dashboard);
        }

        #region Personel Yönetimi

        public ActionResult Staff()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var staff = _context.Users.Where(u => u.Role == "Personel").ToList();
            return View(staff);
        }

        [HttpGet]
        public ActionResult CreateStaff()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStaff(User user)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Şifre kuralları kontrolü
            if (!ValidatePassword(user.Password))
            {
                ModelState.AddModelError("Password", "Şifre en az 8 karakter, 1 büyük harf ve 1 küçük harf içermelidir.");
                return View(user);
            }

            // Kullanıcı adı kontrolü
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Bu kullanıcı adı zaten var!");
                return View(user);
            }

            user.Role = "Personel";
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Personel başarıyla eklendi.";
            return RedirectToAction("Staff");
        }

        [HttpGet]
        public ActionResult EditStaff(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(User user)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Şifre kuralları kontrolü
            if (!ValidatePassword(user.Password))
            {
                ModelState.AddModelError("Password", "Şifre en az 8 karakter, 1 büyük harf ve 1 küçük harf içermelidir.");
                return View(user);
            }

            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null) return HttpNotFound();

            existingUser.Name = user.Name;
            existingUser.Password = user.Password;
            existingUser.Branch = user.Branch;

            _context.SaveChanges();

            TempData["Success"] = "Personel bilgileri güncellendi.";
            return RedirectToAction("Staff");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStaff(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return HttpNotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Success"] = "Personel silindi.";
            return RedirectToAction("Staff");
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(c => char.IsUpper(c));
            bool hasLower = password.Any(c => char.IsLower(c));

            return hasUpper && hasLower;
        }

        #endregion

        #region Kargo Yönetimi

        public ActionResult Cargos(string branch)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var query = _context.Cargos.AsQueryable();

            if (!string.IsNullOrEmpty(branch))
            {
                query = query.Where(c => c.Origin == branch);
            }

            ViewBag.SelectedBranch = branch;
            ViewBag.Branches = _context.Branches.Select(b => b.Name).ToList();

            return View(query.OrderByDescending(c => c.CreateTime).ToList());
        }

        [HttpPost]
        public JsonResult UpdateCargoStatus(int id, string status)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var cargo = _context.Cargos.Find(id);
            if (cargo == null) return Json(new { success = false });

            cargo.Status = status;
            if (status == "Teslim Edildi")
            {
                cargo.DeliveryTime = System.DateTime.Now;
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        #endregion

        #region Raporlar

        public ActionResult Reports()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var dashboard = new DashboardViewModel
            {
                TotalCargos = _context.Cargos.Count(),
                DeliveredCargos = _context.Cargos.Count(c => c.Status == "Teslim Edildi"),
                StaffCount = _context.Users.Count(u => u.Role == "Personel"),
                TotalFeedbacks = _context.Feedbacks.Count(),
                AverageRating = _context.Feedbacks.Any()
                    ? _context.Feedbacks.Average(f => f.Rating)
                    : 0
            };

            return View(dashboard);
        }

        #endregion

        #region Müşteri Yorumları

        public ActionResult Feedbacks()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var feedbacks = _context.Feedbacks
                .OrderByDescending(f => f.CreateTime)
                .ToList();

            return View(feedbacks);
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