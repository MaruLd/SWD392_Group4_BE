using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Posts;
using Application.Posts.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
	public class PostsController : BaseApiController
	{
		/// <summary>
		/// [Authorize] Get Posts
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<Post>>> GetPosts([FromQuery] PostQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		/// <summary>
		/// [Authorize] Get Post
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<Post>> GetPost(Guid id)
		{

			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Create Post
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreatePost(CreatePostDTO Post)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = Post }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Edit Post
		/// </summary>
		[Authorize]
		[HttpPut]
		public async Task<ActionResult> EditPost(EditPostDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		}

		/// <summary>
		/// [Authorize] [>= Moderator] Delete Post
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeletePost([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { id = id }));
		}
	}
}