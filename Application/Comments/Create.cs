
using System.Data.Entity;
using Application.Comments.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Comments
{
  public class Create
  {
    public class Command : IRequest<Result<CommentDTO>>
    {
      public Guid PostId { get; set; }
      public string Body { get; set; }

    }

    public class Handler : IRequestHandler<Command, Result<CommentDTO>>
    {
      private readonly DataContext _context;
      private readonly IUserAccessor _userAccessor;
      private readonly IMapper _mapper;

      public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
      {
        _mapper = mapper;
        _userAccessor = userAccessor;
        _context = context;
      }

      public async Task<Result<CommentDTO>> Handle(Command request, CancellationToken cancellationToken)
      {
        var Post = await _context.Posts.FindAsync(request.PostId);

        if (Post == null) return null;

        var user = await _context.Users
        .SingleOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

        var comment = new Comment
        {
          UserId = user.Id,
          PostId = request.PostId,
          Body = request.Body,
          Status = "Available"
        };

        Post.Comments.Add(comment);

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Result<CommentDTO>.Success(_mapper.Map<CommentDTO>(comment));

        return Result<CommentDTO>.Failure("Failure to add comment");
      }
    }
  }
}