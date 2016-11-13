using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook
{
    public class Helpers
    {
        public static string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA256.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }
    }
}