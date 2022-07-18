using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Application.Core
{
	public class RedisConnection
	{
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
	}
}