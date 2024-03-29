using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.Interfaces;
using Application.Users.DTOs;
using AutoMapper;
using Domain;
using Domain.Enums;
using HtmlCleanser;

namespace Application.Core
{
	public static class HTMLHelper
	{
		public static string CleanupHTML(String rawHtml)
		{
			IHtmlCleanser cleanser = new HtmlCleanser.HtmlCleanser();
			var cleanHtml = cleanser.CleanseFull(rawHtml);
			return cleanHtml;
		}

	}

	public class EventRoleResolver : IMemberValueResolver<Event, SelfEventDTO, List<EventUser>, EventUserTypeEnum>
	{
		private readonly IUserAccessor _userAccessor;

		public EventRoleResolver(IUserAccessor userAccessor)
		{
			this._userAccessor = userAccessor;
		}

		public EventUserTypeEnum Resolve(Event source, SelfEventDTO destination, List<EventUser> sourceMember, EventUserTypeEnum destMember, ResolutionContext context)
		{
			var id = _userAccessor.GetID();
			if (id != Guid.Empty)
			{
				var eu = sourceMember.FirstOrDefault(u => u.UserId == id);
				return eu == null? EventUserTypeEnum.None : eu.Type;
			}
			return EventUserTypeEnum.None;
		}
	}
	
}