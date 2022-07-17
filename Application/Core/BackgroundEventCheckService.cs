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

namespace Application.Core
{
	public class BackgroundEventCheckService : BackgroundService
	{
		private readonly EventRepository _eventRepository;

		private readonly ILogger<BackgroundEventCheckService> _logger;
		private Timer? _timer = null;

		public BackgroundEventCheckService(IServiceProvider services, ILogger<BackgroundEventCheckService> logger)
		{
			this._logger = logger;
			var scope = services.CreateScope();

			_eventRepository = scope.ServiceProvider.GetRequiredService<EventRepository>();
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(5));

			return Task.CompletedTask;
		}

		private async void DoWork(object? state)
		{
			_logger.LogInformation("=================================== Event Ongoing ===================================");

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
					_logger.LogInformation("[ON-GOING] : " + e.Title);
				}

				await _eventRepository.Save();
			}


			if ((DateTime.Now.Hour == 24 || DateTime.Now.Hour == 0) && DateTime.Now.Minute == 0)
			{
				_logger.LogInformation("=================================== Event Ended ===================================");

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
						_logger.LogInformation("[END] : " + e.Title);
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