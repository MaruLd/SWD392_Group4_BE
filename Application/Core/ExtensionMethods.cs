using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;

namespace Application.Core
{
	public class CheckDateRangeAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			DateTime dt = (DateTime)value;
			if (dt >= DateTime.UtcNow)
			{
				return ValidationResult.Success;
			}

			return new ValidationResult(ErrorMessage ?? "Make sure your date is >= than today");
		}

	}

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

		public static bool IsAbleToBuyTicket(this Event e)
		{
			return e.State != EventStateEnum.Draft
				&& e.State != EventStateEnum.Cancelled
				&& e.State != EventStateEnum.Ended;
		}

	}
}