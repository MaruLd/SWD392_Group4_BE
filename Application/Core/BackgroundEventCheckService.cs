using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.Services;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;
using static Application.Core.RedisConnection;

namespace Application.Core
{
	public class BackgroundEventCheckService : BackgroundService
	{
		private readonly EventRepository _eventRepository;

		private readonly ILogger<BackgroundEventCheckService> _logger;
		private readonly RedisConnection _redisConnection;
		private Timer? _timer = null;

		public BackgroundEventCheckService(IServiceProvider services, ILogger<BackgroundEventCheckService> logger)
		{
			this._logger = logger;
			var scope = services.CreateScope();

			_eventRepository = scope.ServiceProvider.GetRequiredService<EventRepository>();
			_redisConnection = scope.ServiceProvider.GetRequiredService<RedisConnection>();
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(5));

			return Task.CompletedTask;
		}

		private async void DoWork(object? state)
		{
			_logger.LogInformation("=================================== Event Autostate ===================================");


			_logger.LogInformation("[>> Checkin]");
			var eventToCheckin = await _eventRepository.GetQuery().Where(e =>
				(
					e.State == EventStateEnum.Publish
				) &&
				e.Status == StatusEnum.Available
				&&
				e.StartTime < DateTime.Now.AddMinutes(30)).ToListAsync();

			if (eventToCheckin.Count > 0)
			{
				foreach (var e in eventToCheckin)
				{
					e.State = EventStateEnum.CheckingIn;
					_eventRepository.Update(e);
					_logger.LogInformation($"[Change] : <{e.Id}> - {e.Title}");
					_redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_All", Data = $"{e.Title} is now ${e.State}" });
				}

				
				await _eventRepository.Save();
			}

			_logger.LogInformation("[>> Ongoing]");
			var eventToOngoing = await _eventRepository.GetQuery().Where(e =>
				(
					e.State == EventStateEnum.Publish ||
					e.State == EventStateEnum.CheckingIn
				) &&
				e.Status == StatusEnum.Available
				&&
				e.StartTime < DateTime.Now).ToListAsync();

			if (eventToOngoing.Count > 0)
			{
				foreach (var e in eventToOngoing)
				{
					e.State = EventStateEnum.Ongoing;
					_eventRepository.Update(e);
					_logger.LogInformation($"[Change] : <{e.Id}> - {e.Title}");
					_redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_All", Data = $"{e.Title} is now ${e.State}" });
				}

				await _eventRepository.Save();
			}


			_logger.LogInformation("[>> Checkout]");
			var eventToCheckout = await _eventRepository.GetQuery().Where(e =>
				(
					e.State == EventStateEnum.Publish
				) &&
				e.Status == StatusEnum.Available
				&&
				e.EndTime > DateTime.Now).ToListAsync();

			if (eventToCheckout.Count > 0)
			{
				foreach (var e in eventToCheckin)
				{
					e.State = EventStateEnum.CheckingOut;
					_eventRepository.Update(e);
					_logger.LogInformation($"[Change] : <{e.Id}> - {e.Title}");
					_redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_All", Data = $"{e.Title} is now ${e.State}" });
				}

				await _eventRepository.Save();
			}

			if ((DateTime.Now.Hour == 24 || DateTime.Now.Hour == 0) && DateTime.Now.Minute == 0)
			{
				_logger.LogInformation("[>> Ended]");
				var eventToEnd = await _eventRepository.GetQuery().Where(e =>
					(
						e.State != EventStateEnum.Draft ||
						e.State != EventStateEnum.Delay ||
						e.State != EventStateEnum.Ended
					) &&
					e.Status == StatusEnum.Available
					&&
					e.EndTime < DateTime.Now).ToListAsync();

				if (eventToEnd.Count > 0)
				{
					foreach (var e in eventToEnd)
					{
						e.State = EventStateEnum.Ended;
						_eventRepository.Update(e);
						_logger.LogInformation($"[Change] : <{e.Id}> - {e.Title}");
						_redisConnection.AddToQueue(new QueueItem() { ActionName = "SendNotification_All", Data = $"{e.Title} is now ${e.State}" });


					}
				}
			}
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}


	}
}