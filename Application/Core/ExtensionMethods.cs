using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;

namespace Application.Core
{
	public static class ExtensionMethods
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			return principal.FindFirstValue("Id");
		}

		public static bool IsModerator(this EventUser eu)
		{
			return eu.Type >= EventUserTypeEnum.Moderator;
		}

		public static bool IsCreator(this EventUser eu)
		{
			return eu.Type == EventUserTypeEnum.Creator;
		}
	}
}