using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JWTDemo2.Ultil
{
    public class TokenManager
    {
        public static string Secret = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        public static string GreneralToken(string username)
        {
            Byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securitykey = new SymmetricSecurityKey(key);

            SecurityTokenDescriptor desc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256),


            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(desc);
            return handler.WriteToken(token);

        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {

                JwtSecurityTokenHandler tokenhandle = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenhandle.ReadJwtToken(token);
                if (jwtToken == null)
                    return null;
                else
                {
                    Byte[] key = Convert.FromBase64String(Secret);
                    TokenValidationParameters parameter = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key)

                    };
                    SecurityToken securityToken;
                    ClaimsPrincipal princ = tokenhandle.ValidateToken(token, parameter, out securityToken);
                    return princ;

                }

            }
            catch
            {
                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal pricaipal = GetPrincipal(token);
            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)pricaipal.Identity;

            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim userClaim = identity.FindFirst(identity.Name);
            username = identity.Name;
            return username;
        }
    }
}