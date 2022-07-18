using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Events.DTOs;
using Application.Services;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Application.Core
{
	public class UserBackgroundService : BackgroundService
	{

		private readonly ILogger<UserBackgroundService> _logger;
		private readonly UserRepository _userRepository;
		private readonly EventUserRepository _eventUserRepository;
		private readonly ConnectionMultiplexer _connection;

		public UserBackgroundService(IServiceProvider services, IConfiguration configuration, ILogger<UserBackgroundService> logger)
		{
			this._logger = logger;
			var scope = services.CreateScope();

			_userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
			_eventUserRepository = scope.ServiceProvider.GetRequiredService<EventUserRepository>();
			_connection = scope.ServiceProvider.GetRequiredService<RedisConnection>().GetConnection();
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_connection.GetSubscriber().Subscribe("UserUpdate", async (channel, message) =>
			{
				var user = JsonSerializer.Deserialize<User>(message);
				_userRepository.Update(user);
				await _userRepository.Save();
			});

			_connection.GetSubscriber().Subscribe("AddUserToEvent", async (channel, message) =>
			{
				var data = JsonSerializer.Deserialize<Dictionary<String, Guid>>(message);

				Guid eventId = data["eventId"];
				Guid userId = data["userId"];

				var eventUser = await _eventUserRepository.GetQuery().FirstOrDefaultAsync(t => t.EventId == eventId && t.UserId == userId);
				if (eventUser == null)
				{
					eventUser = new EventUser() { EventId = eventId, UserId = userId, Type = EventUserTypeEnum.Student };
					_eventUserRepository.Insert(eventUser);
					await _eventUserRepository.Save();
				}				
			});

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");
			return Task.CompletedTask;
		}

		public void Dispose()
		{
		}


	}
}