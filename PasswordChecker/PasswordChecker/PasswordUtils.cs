using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace PasswordChecker
{
    public static class PasswordUtils
    {
        public static bool IsSecure(string password, out string message)
        {
            if (password.Length < 8)
            {
                message = "Le mot de passe doit contenir au moins 8 caractères.";
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                message = "Le mot de passe doit contenir au moins une majuscule.";
                return false;
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                message = "Le mot de passe doit contenir au moins une minuscule.";
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                message = "Le mot de passe doit contenir au moins un chiffre.";
                return false;
            }

            if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\"{}|<>]"))
            {
                message = "Le mot de passe doit contenir au moins un symbole.";
                return false;
            }

            message = "Le mot de passe est sécurisé.";
            return true;
        }

        public static async Task<bool> IsPwnedAsync(string password)
        {
            using (HttpClient client = new HttpClient())
            {
                // Hachage SHA1 du mot de passe
                var sha1 = System.Security.Cryptography.SHA1.Create();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha1.ComputeHash(passwordBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

                // Préparer la requête API (5 premiers caractères du hachage)
                string prefix = hashString.Substring(0, 5);
                string suffix = hashString.Substring(5);
                string url = $"https://api.pwnedpasswords.com/range/{prefix}";

                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                // Vérifier si le suffixe apparaît dans la réponse
                return result.Contains(suffix);
            }
        }

        public static string GenerateSecurePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            return password.ToString();
        }
    }
}
