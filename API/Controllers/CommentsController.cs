using Application.Comments;
using Application.Comments.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers 
{
	[Route("api/v{version:apiVersion}/posts/{postid}/[controller]")]
	[ApiController]
    public class CommentsController : BaseApiController
	{
		// GET: api/<CommentsController>
		[HttpGet]
		public async Task<ActionResult<List<CommentDTO>>> GetComments(Guid postid)
		{
			return HandleResult(await Mediator.Send(new List.Query() { PostId = postid}));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CommentDTO>> GetComment(Guid id)
		{
			return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
		}

		[HttpPost]
		public async Task<ActionResult> CreateComment([FromBody]CreateCommentDTO Comment)
		{
			return HandleResult(await Mediator.Send(new Create.Command { Comment = Comment }));
		}

        [HttpPut()]
        public async Task<ActionResult> EditComment(CommentDTO Comment)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Comment = Comment }));
        }

		[HttpDelete]
		public async Task<ActionResult> DeleteComment([FromBody]Guid id)
		{
			return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
		}
	}
}


