using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using Domain;
using Domain.Enums;
using Stateless;
using static Application.Core.RedisConnection;

namespace Application.Events.StateMachine
{
	public class EventStateMachine
	{
		private StateMachine<int, int> _machine;
		Event _e;
		private readonly RedisConnection redisConnection;

		public EventStateMachine(Event e, RedisConnection redisConnection)
		{
			_e = e;
			this.redisConnection = redisConnection;
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
				.Permit((int)EventStateEnum.CheckingOut, (int)EventStateEnum.CheckingOut);

			_machine.Configure((int)EventStateEnum.CheckingOut)
				.OnEntry(data => OnCheckout())
				.Permit((int)EventStateEnum.Ended, (int)EventStateEnum.Ended);

			_machine.Configure((int)EventStateEnum.Ended)
				.OnEntry(data => OnEnd())
				.Permit((int)EventStateEnum.Draft, (int)EventStateEnum.Draft);

			_machine.Configure((int)EventStateEnum.Cancelled)
				.OnEntry(data => OnCancel());

			void OnDraft()
			{
				_e.State = EventStateEnum.Draft;
			}

			void OnPublish()
			{
				_e.State = EventStateEnum.Publish;
				NotifyAll();
			}

			void OnDelay()
			{
				_e.State = EventStateEnum.Delay;
				NotifyAll();
			}

			void OnCheckin()
			{
				_e.State = EventStateEnum.CheckingIn;
				NotifyAll();
				// NotifySpecific();
			}

			void OnOngoing()
			{
				_e.State = EventStateEnum.Ongoing;
				NotifyAll();
				// NotifySpecific();
			}

			void OnCheckout()
			{
				_e.State = EventStateEnum.CheckingOut;
				NotifyAll();
				// NotifySpecific();
			}

			void OnEnd()
			{
				_e.State = EventStateEnum.Ended;
				NotifyAll();
				// NotifySpecific();
			}

			void OnCancel()
			{
				_e.State = EventStateEnum.Cancelled;
				NotifyAll();
			}
		}

		public Event TriggerState(EventStateEnum eventStateEnum)
		{
			_machine.Fire((int)eventStateEnum);
			return _e;
		}

		private void NotifyAll()
		{
			var dict = new Dictionary<String, Object>();
			dict.Add("message", $"{_e.Title} is now {_e.State}");

			redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_All", Data = dict });
		}

		private void NotifySpecific()
		{

			List<UserFCMToken> tokensToNotify = new List<UserFCMToken>();
			foreach (var t in _e.EventUsers.Select(eu => eu.User.Tokens))
			{
				tokensToNotify.AddRange(t);
			}

			var dict = new Dictionary<String, Object>();
			dict.Add("message", $"{_e.Title} is now {_e.State}");
			dict.Add("list", tokensToNotify);

			redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_Specific", Data = dict });
		}
	}
}