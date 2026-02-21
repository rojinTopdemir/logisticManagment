using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiberKargo.Models
{
    public static class DbInitializer
    {

        public static void Initialize(FiberKargoContext context)
        {
            context.Database.CreateIfNotExists();

            // Şubeler
            if (!context.Branches.Any())
            {
                context.Branches.Add(new Branch { Name = "Adana", PersonelName = "Ahmet Yılmaz", Phone = "0532 111 22 33", Email = "ahmetyilmaz@fiberkargo.com", ImageUrl = "/Content/images/adana.jpeg", Latitude = 37.0, Longitude = 35.32 });
                context.Branches.Add(new Branch { Name = "İstanbul", PersonelName = "Ali Vural", Phone = "0532 222 11 33", Email = "alivural@fiberkargo.com", ImageUrl = "/Content/images/istanbul.jpeg", Latitude = 41.01, Longitude = 28.97 });
                context.Branches.Add(new Branch { Name = "Ankara", PersonelName = "Deniz Yılmaz", Phone = "0532 333 11 33", Email = "denizyilmaz@fiberkargo.com", ImageUrl = "/Content/images/ankara.jpeg", Latitude = 39.93, Longitude = 32.86 });
                context.Branches.Add(new Branch { Name = "İzmir", PersonelName = "Ege Yılmaz", Phone = "0532 444 11 33", Email = "egeyilmaz@fiberkargo.com", ImageUrl = "/Content/images/izmir.jpeg", Latitude = 38.42, Longitude = 27.14 });
                context.Branches.Add(new Branch { Name = "Bursa", PersonelName = "Onur Yılmaz", Phone = "0532 555 11 33", Email = "onuryilmaz@fiberkargo.com", ImageUrl = "/Content/images/bursa.jpeg", Latitude = 40.19, Longitude = 29.06 });
                context.SaveChanges();
            }

            // Kullanıcılar
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Username = "admin", Password = "Admin1234", Name = "Özkan Akseki", Role = "Admin", Branch = null });
                context.Users.Add(new User { Username = "adana", Password = "Adana1234", Name = "Ahmet Yılmaz", Role = "Personel", Branch = "Adana" });
                context.Users.Add(new User { Username = "istanbul", Password = "Istanbul1234", Name = "Ali Vural", Role = "Personel", Branch = "İstanbul" });
                context.Users.Add(new User { Username = "ankara", Password = "Ankara1234", Name = "Deniz Yılmaz", Role = "Personel", Branch = "Ankara" });
                context.Users.Add(new User { Username = "izmir", Password = "Izmir1234", Name = "Ege Yılmaz", Role = "Personel", Branch = "İzmir" });
                context.Users.Add(new User { Username = "bursa", Password = "Bursa1234", Name = "Onur Yılmaz", Role = "Personel", Branch = "Bursa" });
                context.SaveChanges();
            }
        }
    }
}