using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FiberKargo.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [Display(Name = "Ad Soyad")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır")]
        [Display(Name = "Şifre")]
        [StringLength(100)]
        public string Password { get; set; }

        [Display(Name = "Şube")]
        [StringLength(50)]
        public string Branch { get; set; }

        [Display(Name = "Rol")]
        [StringLength(20)]
        public string Role { get; set; } = "Personel";
    }
}