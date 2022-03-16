using System.Security.Cryptography;
using System.Text;

namespace PlayLister.Client.Extensions
{
    public static class HashHelper
    {
        public static string GetHash(string text)
        {
            StringBuilder Sb = new StringBuilder();

            using SHA256 hash = SHA256Managed.Create();
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(text));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));

            return Sb.ToString();
        }
    }
}
