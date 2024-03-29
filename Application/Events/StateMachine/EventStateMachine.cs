using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Stateless;

namespace Application.Events.StateMachine
{
	public class EventStateMachine
	{
		private StateMachine<int, int> _machine;
		Event _e;

		public EventStateMachine(Event e)
		{
			_e = e;
			_machine = new StateMachine<int, int>((int)_e.State);

			_machine.Configure((int)EventStateEnum.Draft)
				.OnEntry(data => OnDraft())
				.Permit((int)EventStateEnum.Publish, (int)EventStateEnum.Publish)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.Publish)
				.OnEntry(data => OnPublish())
				.Permit((int)EventStateEnum.Draft, (int)EventStateEnum.Draft)
				.Permit((int)EventStateEnum.Delay, (int)EventStateEnum.Delay)
				.Permit((int)EventStateEnum.CheckingIn, (int)EventStateEnum.CheckingIn)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.Delay)
				.OnEntry(data => OnDelay())
				.Permit((int)EventStateEnum.Draft, (int)EventStateEnum.Draft)
				.Permit((int)EventStateEnum.Cancelled, (int)EventStateEnum.Cancelled);

			_machine.Configure((int)EventStateEnum.CheckingIn)
				.OnEntry(data => OnCheckin())
				.Permit((int)EventStateEnum.Ongoing, (int)EventStateEnum.Ongoing);

			_machine.Configure((int)EventStateEnum.Ongoing)
				.OnEntry(data => OnOngoing())
				.Permit((int)EventStateEnum.Ongoing, (int)EventStateEnum.CheckingOut);

			_machine.Configure((int)EventStateEnum.CheckingOut)
				.OnEntry(data => OnCheckout())
				.Permit((int)EventStateEnum.Ongoing, (int)EventStateEnum.Ended);

			_machine.Configure((int)EventStateEnum.Ended)
				.OnEntry(data => OnEnd());

			_machine.Configure((int)EventStateEnum.Cancelled)
				.OnEntry(data => OnCancel());

			void OnDraft()
			{
				_e.State = EventStateEnum.Draft;
			}

			void OnPublish()
			{
				_e.State = EventStateEnum.Publish;
			}

			void OnDelay()
			{
				_e.State = EventStateEnum.Delay;
			}

			void OnCheckin()
			{
				_e.State = EventStateEnum.CheckingIn;
			}

			void OnOngoing()
			{
				_e.State = EventStateEnum.Ongoing;
			}

			void OnCheckout()
			{
				_e.State = EventStateEnum.CheckingOut;
			}

			void OnEnd()
			{
				_e.State = EventStateEnum.Ended;
			}

			void OnCancel()
			{
				_e.State = EventStateEnum.Cancelled;
			}
		}

		public Event TriggerState(EventStateEnum eventStateEnum)
		{
			_machine.Fire((int)eventStateEnum);
			return _e;
		}

	}
}