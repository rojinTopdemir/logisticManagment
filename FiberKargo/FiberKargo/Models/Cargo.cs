using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FiberKargo.Models
{
    public class Cargo
    {
        public int Id { get; set; }

        [Display(Name = "Kargo No")]
        [StringLength(50)]
        public string CargoNo { get; set; }

        [Required(ErrorMessage = "Gönderici adi zorunludur")]
        [Display(Name = "Gönderici Ad Soyad")]
        [StringLength(100)]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Gönderici telefon zorunludur")]
        [RegularExpression(@"^5\d{9}$", ErrorMessage = "Telefon 5 ile baslamali ve 10 haneli olmali")]
        [Display(Name = "Gönderici Tel")]
        [StringLength(10)]
        public string SenderTel { get; set; }

        [Required(ErrorMessage = "Alici adi zorunludur")]
        [Display(Name = "Alici Ad Soyad")]
        [StringLength(100)]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Alici telefon zorunludur")]
        [RegularExpression(@"^5\d{9}$", ErrorMessage = "Telefon 5 ile baslamali ve 10 haneli olmali")]
        [Display(Name = "Alici Tel")]
        [StringLength(10)]
        public string ReceiverTel { get; set; }

        [Required(ErrorMessage = "Agirlik zorunludur")]
        [Range(0.1, 1000, ErrorMessage = "Agirlik 0'dan büyük olmalidir")]
        [Display(Name = "Kilo (kg)")]
        public double Weight { get; set; }

        [Display(Name = "En (cm)")]
        public double? Width { get; set; }

        [Display(Name = "Boy (cm)")]
        public double? Length { get; set; }

        [Display(Name = "Yükseklik (cm)")]
        public double? Height { get; set; }

        [Required(ErrorMessage = "Içerik zorunludur")]
        [Display(Name = "Içerik")]
        [StringLength(200)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Ödeme türü zorunludur")]
        [Display(Name = "Ödeme Türü")]
        [StringLength(50)]
        public string PaymentType { get; set; }

        [Display(Name = "Durum")]
        [StringLength(50)]
        public string Status { get; set; } = "Subede";

        [Display(Name = "Olusturulma Tarihi")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Display(Name = "Teslim Tarihi")]
        public DateTime? DeliveryTime { get; set; }

        [Display(Name = "Islemi Yapan")]
        [StringLength(100)]
        public string Handler { get; set; }

        [Display(Name = "Çikis Subesi")]
        [StringLength(50)]
        public string Origin { get; set; }

        [Required(ErrorMessage = "Teslim subesi zorunludur")]
        [Display(Name = "Teslim Subesi")]
        [StringLength(50)]
        public string Destination { get; set; }

        [Display(Name = "Ücret")]
        public double? Price { get; set; }
    }
}