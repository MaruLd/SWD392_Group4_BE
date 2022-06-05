
using System.Data.Entity;
using Application.Comments.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Enums;
using MediatR;
using Persistence;

namespace Application.Comments
{
  public class Create
  {
    public class Command : IRequest<Result<CommentDTO>>
    {
            public CreateCommentDTO Comment { get; set; }
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
        var Post = await _context.Posts.FindAsync(request.Comment.PostId);

        if (Post == null) return null;

        var user = await _context.Users.FindAsync(request.Comment.UserId);

                var comment = new Comment
                {
                    UserId = user.Id,
                    PostId = request.Comment.PostId,
                    Body = request.Comment.Body,
                    Status = StatusEnum.Available
                };

               _context.Comments.Add(comment);


        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Result<CommentDTO>.CreatedSuccess(_mapper.Map<CommentDTO>(comment));

        return Result<CommentDTO>.Failure("Failure to add comment");
      }
    }
  }
}