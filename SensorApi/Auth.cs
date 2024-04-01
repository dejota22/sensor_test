using Core;
using Core.Service;
using global::SensorApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SensorApi
{
    public class Auth : IJwtAuth
    {
        private readonly IUserService _userService;

        private readonly string key;
        public Auth(IUserService userService)
        {
            this.key = "3FJRhIppOMZ3Z0MkKsBfiej4M9Ms1j5k";
            _userService = userService;
        }        
        public string Authentication(string username, string password)
        {            

            var passCrypto = MD5Hash.CalculaHash(password);
            var userLogin = _userService.Login(username, passCrypto);

            if (userLogin is null)
            {
                return null;
            }

            if (userLogin != null && userLogin.IsActive == 0)
            {
                return null;
            }            

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}