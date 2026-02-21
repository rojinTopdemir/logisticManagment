using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FiberKargo.Services
{
    public class CaptchaService
    {
        private const string Characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly Random Random = new Random();

        public string GenerateCaptcha()
        {
            var captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                captcha.Append(Characters[Random.Next(Characters.Length)]);
            }
            return captcha.ToString();
        }

        public bool ValidateCaptcha(string userInput, string storedCaptcha)
        {
            return userInput == storedCaptcha;
        }
    }
}