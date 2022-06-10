using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Stateless;

namespace Application.Events.StateMachine
{

	public enum EventTriggerEnum
	{
		Start, End
	}

	public class EventStateMachine
	{
		private StateMachine<int, int> _machine;
		Event _e;

		public EventStateMachine(Event e)
		{
			_e = e;
			_machine = new StateMachine<int, int>((int)_e.State);

			_machine.Configure((int)EventStateEnum.Draft)
				.OnEntry(data => { _e.State = EventStateEnum.Draft; })
				.Permit((int)EventStateEnum.Publish, (int)EventStateEnum.Publish)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.Publish)
				.OnEntry(data => { _e.State = EventStateEnum.Publish; })
				.Permit((int)EventStateEnum.Delay, (int)EventStateEnum.Delay)
				.Permit((int)EventStateEnum.EarlyCheckin, (int)EventStateEnum.EarlyCheckin)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.Delay)
				.OnEntry(data => { _e.State = EventStateEnum.Delay; })
				.Permit((int)EventStateEnum.Draft, (int)EventStateEnum.Draft)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.Ongoing)
				.OnEntry(data => OnStart())
				.Permit((int)EventTriggerEnum.End, (int)EventStateEnum.Ended);

			_machine.Configure((int)EventStateEnum.Ended)
				.OnEntry(data => OnEnd());

			void OnStart()
			{
				_e.State = EventStateEnum.Ongoing;
			}

			void OnEnd()
			{
				_e.State = EventStateEnum.Ended;
			}
		}

		public Event TriggerState(EventTriggerEnum eventTriggerEnum)
		{
			_machine.Fire((int)eventTriggerEnum);
			return _e;
		}

	}
}