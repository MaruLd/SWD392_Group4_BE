using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments;
using Application.Comments.DTOs;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
	public class ChatHub : Hub
	{
		private readonly IMediator _mediator;
		private readonly PostConnections _postConnections;

		public ChatHub(IMediator mediator, PostConnections postConnections, CommentService commentService)
		{
			_mediator = mediator;
			this._postConnections = postConnections;
		}

		public async Task SendComment(String content)
		{

			var httpContext = Context.GetHttpContext();

			var cmd = new Create.Command()
			{
				postid = _postConnections.GetPostIdFromConnection(Context.ConnectionId),
				dto = new CreateCommentDTO() { Body = content }
			};

			var comment = await _mediator.Send(cmd);
			var postId = _postConnections.GetPostIdFromConnection(Context.ConnectionId);
			var connections = _postConnections.GetConnectionsInPost(postId);

			// await Clients.All.SendAsync("NewComment", comment.Value);
			foreach (var c in connections)
			{
				var client = Clients.Client(c);
				await client.SendAsync("NewComment", comment.Value);
			}
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var postId = httpContext.Request.Query["postId"];

			_postConnections.AddConnection(Context.ConnectionId, Guid.Parse(postId));

			await Clients.Caller.SendAsync("Load", "Welcome");

			var result = await _mediator.Send(new List.Query { postId = Guid.Parse(postId), queryParams = new CommentQueryParams() });
			await Clients.Caller.SendAsync("LoadComments", result.Value);
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var httpContext = Context.GetHttpContext();
			_postConnections.RemoveConnection(Context.ConnectionId);
		}
	}
}