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
	[Authorize]
	public class PostsController : BaseApiController
	{
		[HttpGet]
		public async Task<ActionResult<List<Post>>> GetPosts([FromQuery] PostQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { queryParams = queryParams }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Post>> GetPost(Guid id)
		{

			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreatePost(CreatePostDTO Post)
		{
			return HandleResult(await Mediator.Send(new Create.Command { dto = Post }));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<ActionResult> EditPost(Guid id, EditPostDTO dto)
		{
			return HandleResult(await Mediator.Send(new Edit.Command { postId = id, dto = dto }));
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<ActionResult> DeletePost(Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}