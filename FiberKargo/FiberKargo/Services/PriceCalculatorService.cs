using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiberKargo.Services
{
    public class PriceCalculatorService
    {
        private static readonly Dictionary<string, (double lat, double lon)> CityCoords = new Dictionary<string, (double lat, double lon)>
        {
            { "Adana", (37.0, 35.32) },
            { "İstanbul", (41.01, 28.97) },
            { "Ankara", (39.93, 32.86) },
            { "İzmir", (38.42, 27.14) },
            { "Bursa", (40.19, 29.06) }
        };

        private const double BaseFee = 30;
        private const double PricePerKm = 0.6;
        private const double PricePerKg = 18;
        private const double DesiDivisor = 3000;

        public double CalculateDistance(string fromCity, string toCity)
        {
            if (fromCity == toCity) return 0;

            if (!CityCoords.ContainsKey(fromCity) || !CityCoords.ContainsKey(toCity))
                return 0;

            var from = CityCoords[fromCity];
            var to = CityCoords[toCity];

            const double R = 6371;

            var lat1 = from.lat * Math.PI / 180;
            var lon1 = from.lon * Math.PI / 180;
            var lat2 = to.lat * Math.PI / 180;
            var lon2 = to.lon * Math.PI / 180;

            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Pow(Math.Sin(dLon / 2), 2);

            return 2 * R * Math.Asin(Math.Sqrt(a));
        }

        public double CalculateDesi(double width, double length, double height)
        {
            return (width * length * height) / DesiDivisor;
        }

        public double CalculatePrice(string fromCity, string toCity, double weight, double width, double length, double height)
        {
            var distance = CalculateDistance(fromCity, toCity);
            var desi = CalculateDesi(width, length, height);
            var chargeWeight = Math.Max(weight, desi);

            return BaseFee + (distance * PricePerKm) + (chargeWeight * PricePerKg);
        }
    }
}