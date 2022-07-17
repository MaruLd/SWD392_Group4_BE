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
	public class BackgroundEventCheckService : IHostedService, IDisposable
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

		public Task StartAsync(CancellationToken stoppingToken)
		{
			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(1));

			return Task.CompletedTask;
		}

		private async void DoWork(object? state)
		{

			var events = await _eventRepository.GetQuery().Where(e =>
				(
					e.State == EventStateEnum.Publish ||
					e.State == EventStateEnum.CheckingIn
				) &&
				e.Status == StatusEnum.Available
				&&
				e.StartTime < DateTime.Now).ToListAsync();

			_logger.LogDebug("Event taht need help : " + events.Count);


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