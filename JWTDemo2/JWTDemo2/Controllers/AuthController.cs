using JWTDemo2.Ultil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWTDemo2.Controllers
{
    [Route("api/Auth/{action}", Name = "Auth")]

    public class AuthController : ApiController
    {
        // GET: api/Auth
        [HttpGet]
        [CustomAuthenticationFilter]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public string Post(string username, string password)
        {
            if (username == "nghia" && password == "nghia")
            {
                return TokenManager.GreneralToken(username);

            }
            return "BadGateway";
        }

        // GET: api/Auth/5

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Auth
        [HttpGet]
        public string GetToken(string username, string password)
        {
            if (username == "nghia" && password == "nghia")
            {
                return TokenManager.GreneralToken(username);

            }
            return "BadGateway";
        }

        // PUT: api/Auth/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Auth/5
        public void Delete(int id)
        {
        }
    }
}
