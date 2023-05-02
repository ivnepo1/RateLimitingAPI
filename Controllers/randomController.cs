using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace ApiRandomBytes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class randomController : ControllerBase
    {
        private readonly object counterLock = new object();
        private static int ByteCounter = 0;
        private static int MaxRequests = 1024;
        private int MinRequestsRange = 100;
        private int MaxRequestsRange = 1048576;
        private int DefaultBytes = 32;
        private static DateTime RateWindowStart = DateTime.UtcNow;
        public int WindowTimeInSeconds = 10;

        [JwtAuthentication]
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

        [JwtAuthentication]
        [HttpPost("limit")]
        public IActionResult UpdateLimit([FromBody] RequestLimitClass request)
        {
            if (request.limit <= 0 || request.limit < MinRequestsRange || request.limit > MaxRequestsRange)
            {
                return StatusCode(429, $"Max request limit has been updated to {request.limit}.");
            } else
            {
                MaxRequests = request.limit;
                return Ok($"Max request limit has been updated to {request.limit}.");
            }

        }
        [JwtAuthentication]
        [HttpPost("reset")]
        public IActionResult Reset()
        {
            ByteCounter = 0;

            return Ok("reset complete :D");

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.username == "1" && model.password == "simcorp")
            {
                var token = JwtTokenHelper.CreateToken(model.username); 
                return Ok(new { Token = token });
            } else
            {
                return StatusCode(401, "Incorrect username and/or password");
            }
        }
    }
}

