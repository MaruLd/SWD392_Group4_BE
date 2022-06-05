using Application.Comments;
using Application.Comments.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers 
{
	[Route("api/v{version:apiVersion}/event/{event-id}/post/{post-id}/comment")]
	[ApiVersion("1.0")]
	[ApiController]
    public class CommentsController : BaseApiController
	{
		// GET: api/<CommentsController>
		[HttpGet]
		public async Task<ActionResult<List<CommentDTO>>> GetComments([FromQuery] Guid PostId)
		{
			return HandleResult(await Mediator.Send(new List.Query() { PostId = PostId }));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CommentDTO>> GetComment(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult> CreateComment(CreateCommentDTO Comment)
		{
			return HandleResult(await Mediator.Send(new Create.Command { Comment = Comment }));
		}

		//[Authorize(Roles = "Admin")]
		//[HttpPut()]
		//public async Task<ActionResult> EditComment(EditCommentDTO Comment)
		//{
		//	return HandleResult(await Mediator.Send(new Edit.Command { CommentId = id, Comment = Comment }));
		//}

		[Authorize(Roles = "Admin")]
		[HttpDelete()]
		public async Task<ActionResult> DeleteComment(Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}


