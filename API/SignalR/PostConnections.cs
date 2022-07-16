using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
	public class PostConnections
	{
		private Dictionary<String, Guid> Connections { get; set; } = new Dictionary<string, Guid>();

		public void AddConnection(String connectionId, Guid postId)
		{
			Connections.Add(connectionId, postId);
		}

		public void RemoveConnection(String connectionId)
		{
			Connections.Remove(connectionId);
		}

		public List<String> GetConnectionsInPost(Guid postId)
		{
			return Connections.Where(p => postId == p.Value).ToList().Select(v => v.Key).ToList();
		}

		public Guid GetPostIdFromConnection(string connectionId)
		{
			return Connections[connectionId];
		}
	}
}