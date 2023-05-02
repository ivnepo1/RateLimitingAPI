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

    public class RequestLimitClass
    {
        public int limit { get; set; }
    }

    public class ClientRateLimitClass
    {
        public string id { get; set; }
        public int maxRequests { get; set; }
    }

    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}