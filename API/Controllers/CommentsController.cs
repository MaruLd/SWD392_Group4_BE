using Application.Comments;
using Application.Comments.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
	[Route("api/v{version:apiVersion}/posts/{postid}/comments")]
	[ApiVersion("1.0")]
	public class CommentsController : BaseApiController
	{
		/// <summary>
		/// Get Comments
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<List<CommentDTO>>> GetComments(Guid postid, [FromQuery] CommentQueryParams queryParams)
		{
			return HandleResult(await Mediator.Send(new List.Query() { postId = postid, queryParams = queryParams }));
		}

		/// <summary>
		/// Get Comment
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<CommentDTO>> GetComment(Guid postid)
		{
			return HandleResult(await Mediator.Send(new Details.Query { postId = postid }));
		}

		/// <summary>
		/// [Authorize] [Student] Create Comment
		/// </summary>
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> CreateComment(Guid postid, [FromBody] CreateCommentDTO dto)
		{
			return HandleResult(await Mediator.Send(new Create.Command { postid = postid, dto = dto }));
		}

		/// <summary>
		/// [Authorize] [Student] Write Comment
		/// </summary>
		// [Authorize]
		// [HttpPut]
		// public async Task<ActionResult> EditComment(Guid postid, EditCommentDTO dto)
		// {
		// 	dto.PostId = postid;
		// 	return HandleResult(await Mediator.Send(new Edit.Command { dto = dto }));
		// }

		/// <summary>
		/// [Authorize] [>= Moderator] Delete Comment
		/// </summary>
		[Authorize]
		[HttpDelete]
		public async Task<ActionResult> DeleteComment([FromBody] Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { commentId = id }));
		}
	}
}


