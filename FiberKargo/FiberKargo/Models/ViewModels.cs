using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FiberKargo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Güvenlik kodu zorunludur")]
        [Display(Name = "Güvenlik Kodu")]
        public string Captcha { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalCargos { get; set; }
        public int DeliveredCargos { get; set; }
        public int StaffCount { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
    }

    public class PriceCalculatorViewModel
    {
        [Required(ErrorMessage = "Gönderim şehri zorunludur")]
        [Display(Name = "Gönderim Şehri")]
        public string FromCity { get; set; }

        [Required(ErrorMessage = "Teslim şehri zorunludur")]
        [Display(Name = "Teslim Şehri")]
        public string ToCity { get; set; }

        [Required(ErrorMessage = "Ağırlık zorunludur")]
        [Range(0.1, 1000, ErrorMessage = "Ağırlık 0'dan büyük olmalıdır")]
        [Display(Name = "Ağırlık (kg)")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "En zorunludur")]
        [Range(0.1, 1000, ErrorMessage = "En 0'dan büyük olmalıdır")]
        [Display(Name = "En (cm)")]
        public double Width { get; set; }

        [Required(ErrorMessage = "Boy zorunludur")]
        [Range(0.1, 1000, ErrorMessage = "Boy 0'dan büyük olmalıdır")]
        [Display(Name = "Boy (cm)")]
        public double Length { get; set; }

        [Required(ErrorMessage = "Yükseklik zorunludur")]
        [Range(0.1, 1000, ErrorMessage = "Yükseklik 0'dan büyük olmalıdır")]
        [Display(Name = "Yükseklik (cm)")]
        public double Height { get; set; }

        public double? Distance { get; set; }
        public double? Desi { get; set; }
        public double? TotalPrice { get; set; }
    }
}