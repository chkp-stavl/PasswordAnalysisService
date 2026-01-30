using Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public sealed class Sha1PasswordHasher : IPasswordHasher
    {
        public string Hash(string input)
        {
            using var sha1 = SHA1.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToHexString(sha1.ComputeHash(bytes));
        }
    }
}
