using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain;
using Domain.Enums;
using Stateless;

namespace Application.Events
{
	public class EventStateMachine
	{
		StateMachine<int, int> _machine;
		Event _event;

		public EventStateMachine(Event e)
		{
			this._event = e;

			_machine = new StateMachine<int, int>((int)this._event.State);

			var triggerWithParam_Start = _machine.SetTriggerParameters<TicketUser>((int)EventTriggerEnum.Start);
			var triggerWithParam_End = _machine.SetTriggerParameters<TicketUser>((int)EventTriggerEnum.End);

			_machine.Configure((int)EventStateEnum.Idle)
				.Permit((int)EventTriggerEnum.Start, (int)EventStateEnum.Started);

			_machine.Configure((int)EventStateEnum.Started)
				.OnEntryFrom(triggerWithParam_Start, data => OnStart())
				.Permit((int)EventTriggerEnum.End, (int)EventStateEnum.Ended);

			_machine.Configure((int)EventStateEnum.Ended)
				.OnEntryFrom(triggerWithParam_Start, data => OnEnd());
		}

		void OnStart()
		{
			_event.State = EventStateEnum.Started;
		}

		void OnEnd()
		{
			_event.State = EventStateEnum.Ended;
		}

		public Event Start()
		{
			_machine.Fire((int)EventTriggerEnum.Start);
			return _event;
		}

		public Event End()
		{
			_machine.Fire((int)EventTriggerEnum.End);
			return _event;
		}
	}
}