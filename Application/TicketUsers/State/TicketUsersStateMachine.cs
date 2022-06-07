using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

		public TicketUsersStateMachine(TicketUser ticketUser)
		{
			this._ticketUser = ticketUser;

			_machine = new StateMachine<int, int>((int)this._ticketUser.State);

			var triggerWithParam_CheckIn = _machine.SetTriggerParameters<TicketUser>((int)TicketUserTriggerEnum.CheckIn);
			var triggerWithParam_CheckOut = _machine.SetTriggerParameters<TicketUser>((int)TicketUserTriggerEnum.CheckOut);
			var triggerWithParam_End = _machine.SetTriggerParameters<TicketUser>((int)TicketUserTriggerEnum.End);

			_machine.Configure((int)TicketUserStateEnum.Idle)
				.Permit((int)TicketUserTriggerEnum.CheckIn, (int)TicketUserStateEnum.CheckedIn)
				.Permit((int)TicketUserTriggerEnum.End, (int)TicketUserStateEnum.Ended);

			_machine.Configure((int)TicketUserStateEnum.CheckedIn)
				.OnEntryFrom(triggerWithParam_CheckIn, data => OnCheckIn())
				.Permit((int)TicketUserTriggerEnum.CheckOut, (int)TicketUserStateEnum.CheckedOut)
				.Permit((int)TicketUserTriggerEnum.End, (int)TicketUserStateEnum.Ended);

			_machine.Configure((int)TicketUserStateEnum.CheckedOut)
				.Permit((int)TicketUserTriggerEnum.End, (int)TicketUserStateEnum.Ended);
		}

		void OnCheckIn()
		{
			_ticketUser.CheckedInDate = DateTime.Now;
			_ticketUser.State = TicketUserStateEnum.CheckedIn;
		}

		void OnCheckOut()
		{
			_ticketUser.CheckedOutDate = DateTime.Now;
			_ticketUser.State = TicketUserStateEnum.CheckedOut;
		}

		void OnEnd()
		{
			_ticketUser.State = TicketUserStateEnum.Ended;
		}

		public TicketUser CheckIn()
		{
			_machine.Fire((int)TicketUserTriggerEnum.CheckIn);
			return _ticketUser;
		}

		public TicketUser CheckOut()
		{
			_machine.Fire((int)TicketUserTriggerEnum.CheckOut);
			return _ticketUser;
		}

		public TicketUser End()
		{
			_machine.Fire((int)TicketUserTriggerEnum.End);
			return _ticketUser;
		}
	}
}