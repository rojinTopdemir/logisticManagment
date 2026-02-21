using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FiberKargo.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [Display(Name = "Ad Soyad")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Şube seçimi zorunludur")]
        [Display(Name = "Şube")]
        [StringLength(50)]
        public string Branch { get; set; }

        [Required(ErrorMessage = "Puan zorunludur")]
        [Range(1, 5, ErrorMessage = "Puan 1-5 arasında olmalıdır")]
        [Display(Name = "Puan")]
        public int Rating { get; set; }

        [Display(Name = "Yorum")]
        [StringLength(500)]
        public string Comment { get; set; }

        [Display(Name = "Tarih")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}