using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
	public class TokenService
	{
		private readonly SymmetricSecurityKey _key;
		private readonly UserManager<User> _userManager;

		public TokenService(IConfiguration config, UserManager<User> userManager)
		{
			var secret = config.GetValue<string>("Authentication:JWTSecretKey");
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			this._userManager = userManager;
		}

		public async Task<string> CreateToken(User user)
		{
			var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
			return await CreateToken(user, role);
		}

		public async Task<string> CreateToken(User user, string role)
		{
			var claims = new Claim[]
				{
					new Claim("id", user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					new Claim("name", user.DisplayName),
					new Claim(ClaimTypes.Role, role)
				};

			var tokenHandler = new JwtSecurityTokenHandler();
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(30),
				SigningCredentials = creds,

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public string CreateTestToken(string email)
		{
			var claims = new Claim[]
				{
					new Claim(JwtRegisteredClaimNames.Email, email),
				};

			var tokenHandler = new JwtSecurityTokenHandler();
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds,

			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}