using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
	public class UserAccessor : IUserAccessor
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetUsername()
		{
			return _httpContextAccessor.HttpContext.User.FindFirstValue("name");
		}

		public string GetEmail()
		{
			return _httpContextAccessor.HttpContext.User.FindFirstValue("email");
		}

		public string GetRole()
		{
			return _httpContextAccessor.HttpContext.User.FindFirstValue("role");
		}
	}
}