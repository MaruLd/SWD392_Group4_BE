using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Application.Core
{
	public class RedisConnection
	{
		public class QueueItem
		{
			public string ActionName { get; set; }
			public Object Data { get; set; }
		}

		public ConnectionMultiplexer _connection { get; }

		public RedisConnection(IConfiguration configuration)
		{
			ConfigurationOptions options = new ConfigurationOptions()
			{
				EndPoints = { { configuration.GetValue<String>("redis:host"), 19882 } },
				AllowAdmin = true,
				User = configuration.GetValue<String>("redis:username"),
				Password = configuration.GetValue<String>("redis:password"),
			};

			_connection = ConnectionMultiplexer.Connect(options);
		}

		public ConnectionMultiplexer GetConnection() => _connection;

		public void AddToQueue(String actionName, Object data)
		{
			AddToQueue(new QueueItem() { ActionName = actionName, Data = data });
		}

		public void AddToQueue(QueueItem qi)
		{
			var db = _connection.GetDatabase().ListRightPushAsync("Action_Queue", JsonSerializer.Serialize(qi));
			_connection.GetSubscriber().Publish("QueueLocal", JsonSerializer.Serialize(qi));
		}

		public async Task<QueueItem> GetFromQueue()
		{
			var res = await _connection.GetDatabase().ListLeftPopAsync("Action_Queue");
			if (res.IsNull) return null;

			var qi = JsonSerializer.Deserialize<QueueItem>(res);
			return qi;
		}
	}
}