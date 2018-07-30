using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RarePuppers.Services
{
    public static class HashString
    {
        public static string Hash(string input)
        {
            //set up the hashing method
            HashAlgorithm algo = SHA256.Create();
            
            //now turn to byte code
            byte[] byteArray = algo.ComputeHash(Encoding.UTF8.GetBytes(input));
            //now hash that b
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteArray)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
