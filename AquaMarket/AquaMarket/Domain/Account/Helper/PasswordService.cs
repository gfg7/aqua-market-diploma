using AquaServer.Interfaces.Services;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AquaServer.Domain.Account.Actions.Helper
{
    public class PasswordService : IPasswordService
    {
        public bool Compare(string hash, string salt, string input)
        {
            var md = MD5.Create();
            input += salt.Trim();
            string hashPass = Convert.ToBase64String(md.ComputeHash(Encoding.Unicode.GetBytes(input)));

            return hash.Trim() == hashPass;
        }

        public (string, string) Generate(string pass = null)
        {
            if (pass is null)
            {
                pass = Guid.NewGuid().ToString("N")[..10];
            }

            var buf = new byte[16];
            new Random().NextBytes(buf);

            string salt = Convert.ToBase64String(buf);
            pass += salt;
            var md = MD5.Create();
            string hash = Convert.ToBase64String(md.ComputeHash(Encoding.Unicode.GetBytes(pass)));

            return (hash, salt);
        }
    }
}
