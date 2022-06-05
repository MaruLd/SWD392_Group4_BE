using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
    public class EditCommentDTO
    {

        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        
        public string Body { get; set; }
        public StatusEnum Status { get; set; }

    }
}