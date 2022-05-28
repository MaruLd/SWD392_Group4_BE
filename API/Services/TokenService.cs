using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
	public class TokenService
	{
		private readonly SymmetricSecurityKey _key;
		
		public TokenService(IConfiguration config)
		{
			var secret = config.GetValue<string>("Authentication:JWTSecretKey");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

		public string CreateToken(string email)
		{
			var claims = new Claim[]
				{
			new Claim(JwtRegisteredClaimNames.Email, email)
				};

			var tokenHandler = new JwtSecurityTokenHandler();
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = creds,

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}