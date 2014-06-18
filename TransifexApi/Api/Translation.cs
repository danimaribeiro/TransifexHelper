using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TransifexApi.Api
{
    public class Translation
    {
        public string comment { get; set; }
        public string context { get; set; }
        public string key { get; set; }
        public bool reviewed { get; set; }
        public bool pluralized { get; set; }
        public string source_string { get; set; }
        public string translation { get; set; }

        public string CalculateHash()
        {
            string identifier = key + ":" + context;
            byte[] hash = MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(identifier));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public class TranslationUpdate
    {  
        public string translation { get; set; }
    }
}
