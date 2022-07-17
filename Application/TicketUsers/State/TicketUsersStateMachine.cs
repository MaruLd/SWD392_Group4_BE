using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Domain;
using Domain.Enums;
using Stateless;

namespace Application.TicketUsers
{
	public class TicketUsersStateMachine
	{
		StateMachine<int, int> _machine;
		TicketUser _ticketUser;
		private readonly EventService _eventService;
		private readonly UserService _userService;

		public TicketUsersStateMachine(TicketUser ticketUser, EventService eventService, UserService userService)
		{
			this._ticketUser = ticketUser;
			this._eventService = eventService;
			this._userService = userService;
			_machine = new StateMachine<int, int>((int)this._ticketUser.State);

			_machine.Configure((int)TicketUserStateEnum.Idle)
				.Permit((int)TicketUserStateEnum.CheckedIn, (int)TicketUserStateEnum.CheckedIn)
				.Permit((int)TicketUserStateEnum.Ended, (int)TicketUserStateEnum.Ended);

			_machine.Configure((int)TicketUserStateEnum.CheckedIn)
				.OnEntry(data => OnCheckIn())
				.Permit((int)TicketUserStateEnum.CheckedOut, (int)TicketUserStateEnum.CheckedOut)
				.Permit((int)TicketUserStateEnum.Ended, (int)TicketUserStateEnum.Ended);

			_machine.Configure((int)TicketUserStateEnum.CheckedOut)
				.OnEntry(data => OnCheckOut())
				.Permit((int)TicketUserStateEnum.Ended, (int)TicketUserStateEnum.Ended);

			_machine.Configure((int)TicketUserStateEnum.Ended)
				.OnEntry(data => OnEnd());

		}

		void OnCheckIn()
		{
			if (!_ticketUser.Ticket.Event.IsAbleToCheckin()) throw new Exception();
			_ticketUser.CheckedInDate = DateTime.Now;
			_ticketUser.State = TicketUserStateEnum.CheckedIn;
		}

		void OnCheckOut()
		{
			if (!_ticketUser.Ticket.Event.IsAbleToCheckout()) throw new Exception();
			_ticketUser.CheckedOutDate = DateTime.Now;
			_ticketUser.State = TicketUserStateEnum.CheckedOut;

			new Thread(async () =>
			{
				var e = await _eventService.GetByID(_ticketUser.Ticket.EventId.Value);

				var user = _ticketUser.User;
				var baseBonus = _ticketUser.Ticket.Cost;

				if (e.StartTime < _ticketUser.CheckedInDate)
				{
					var lossPercentage = (e.EndTime - _ticketUser.CheckedInDate)
											/
										(e.StartTime - e.EndTime);

					baseBonus = (float)(baseBonus + (baseBonus * e.MultiplierFactor * lossPercentage ));
				} else {
					baseBonus = (float)(baseBonus + (baseBonus * e.MultiplierFactor));
				}

				user.Bean += baseBonus;

				await _userService.Update(user);
			}).Start();
		}

		void OnEnd()
		{
			_ticketUser.State = TicketUserStateEnum.Ended;
		}

		public TicketUser TriggerState(TicketUserStateEnum ticketUserStateEnum)
		{
			_machine.Fire((int)ticketUserStateEnum);
			return _ticketUser;
		}
	}
}