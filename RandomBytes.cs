using System;
using System.Text;
using System.Text.Json;

namespace ApiRandomBytes
{
    public class randomReturnClass
    {
        public string random { get; set; }
        //public int? len { get; set; }
        //public int byteCounter { get; set; }
        //public double timeDifference { get; set; }
    }

    public class RandomProgram
    {
        public static string Base64Encode(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }
        public static byte[] Base64Decode(string base64String)
        {
            return Convert.FromBase64String(base64String);
        }

        public static byte[] GetRandomBytes(int length)
        {
            Random rnd = new Random();
            var bytes = new byte[length];
            rnd.NextBytes(bytes);
            return bytes;
        }

    }
}