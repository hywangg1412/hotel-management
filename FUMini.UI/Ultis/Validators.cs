using System.Text.RegularExpressions;

namespace FUMini.UI.Ultis
{
    public static class Validators
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone ?? "", @"^\d{10,11}$");
        }
    }
}
