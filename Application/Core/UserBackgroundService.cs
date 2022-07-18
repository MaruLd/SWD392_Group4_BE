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
using static Application.Core.RedisConnection;

namespace Application.Core
{
	public class RedisQueueBackgroundService : BackgroundService
	{
		private readonly ILogger<RedisQueueBackgroundService> _logger;
		private readonly UserRepository _userRepository;
		private readonly EventUserRepository _eventUserRepository;
		private readonly RedisConnection _redisConnection;

		private bool IsRunning = false;

		public RedisQueueBackgroundService(IServiceProvider services, IConfiguration configuration, ILogger<RedisQueueBackgroundService> logger)
		{
			this._logger = logger;
			var scope = services.CreateScope();

			_userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
			_eventUserRepository = scope.ServiceProvider.GetRequiredService<EventUserRepository>();
			_redisConnection = scope.ServiceProvider.GetRequiredService<RedisConnection>();
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_redisConnection.GetConnection().GetSubscriber().Subscribe("Queue", async (channel, message) =>
			{
				if (!IsRunning)
				{
					IsRunning = true;
					await DoNextWork();
				};
			});

			return Task.CompletedTask;
		}

		public async Task DoNextWork()
		{
			var work = await _redisConnection.GetFromQueue();
			if (work != null)
			{
				switch (work.ActionName)
				{
					case "UserUpdate":
						{
							var user = JsonSerializer.Deserialize<User>(work.Data.ToString());
							_userRepository.Update(user);
							await _userRepository.Save();
							break;
						}

					case "EventAddUser":
						{
							var data = JsonSerializer.Deserialize<Dictionary<String, Guid>>(work.Data.ToString());

							Guid eventId = data["eventId"];
							Guid userId = data["userId"];

							var eventUser = await _eventUserRepository.GetQuery().Where(t => t.EventId == eventId && t.UserId == userId).FirstOrDefaultAsync();
							if (eventUser == null)
							{
								eventUser = new EventUser() { EventId = eventId, UserId = userId, Type = EventUserTypeEnum.Student };
								_eventUserRepository.Insert(eventUser);
								await _eventUserRepository.Save();
							}
							break;
						}
				}
				await DoNextWork();
			}
			else
			{
				IsRunning = false;
			}
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
		}


	}
}