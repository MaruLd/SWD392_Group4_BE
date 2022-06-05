using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Core
{
	public static class ExtensionMethods
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			return principal.FindFirstValue("Id");
		}
	}
}