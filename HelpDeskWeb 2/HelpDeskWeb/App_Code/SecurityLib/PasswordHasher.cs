using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace SecurityLib
{
    //create the password hasher
    public static class PasswordHasher
    {
        //(Secure Hash Algorithm) from the .NET Framework can change SHA1Managed to SHA1512Managed to create
        //a huuuuge hash (512 bit), Don't really need it in this case though
        private static SHA1Managed hasher = new SHA1Managed();
        public static string Hash(string password)
        {
            // convert password to byte array
            byte[] passwordBytes =
            System.Text.ASCIIEncoding.ASCII.GetBytes(password);
            // generate hash from byte array of password
            byte[] passwordHash = hasher.ComputeHash(passwordBytes);
            // convert hash to string
            return Convert.ToBase64String(passwordHash, 0, passwordHash.Length);
        }
    }
}