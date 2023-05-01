using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiRandomBytes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class randomController : ControllerBase
    {
        private readonly object counterLock = new object();
        private static int ByteCounter = 0;
        private int MaxRequests = 1024;
        private int DefaultBytes = 32;
        private static DateTime RateWindowStart = DateTime.UtcNow;
        public int WindowTimeInSeconds = 10;

        [HttpGet]
        public async Task<IActionResult> RateLimitedGetRandomBytes(int? len)
        {
            int actualLen = len ?? DefaultBytes;
            double dateTimeDifference = 0.0;

            if (actualLen > MaxRequests)
            {
                return BadRequest("Request too large");
            }
            lock (counterLock)
            {
                var now = DateTime.UtcNow;
                dateTimeDifference = (now - RateWindowStart).TotalSeconds;
                var remainingRequests = MaxRequests - ByteCounter;

                if (dateTimeDifference > WindowTimeInSeconds)
                {
                    ByteCounter = 0;
                    RateWindowStart = DateTime.UtcNow;
                }

                if ((ByteCounter + actualLen) > MaxRequests)
                {
                    return StatusCode(429, $"Exceeded by {ByteCounter + actualLen - MaxRequests} bytes");
                }
                else
                {
                    Response.Headers.Add("X-Rate-Limit", remainingRequests.ToString());
                }

                ByteCounter += actualLen;
            }

            byte[] preEncodeBytesArray = RandomProgram.GetRandomBytes(actualLen);
            var objectToReturn = new randomReturnClass 
            { 
                random = RandomProgram.Base64Encode(preEncodeBytesArray)
            };
            var jsonSerialToReturn = JsonSerializer.Serialize(objectToReturn);
            return Content(jsonSerialToReturn, "application/json");

        }
    }
}

