using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string Body { get; set; }
        
        public StatusEnum Status { get; set; }
    }
}