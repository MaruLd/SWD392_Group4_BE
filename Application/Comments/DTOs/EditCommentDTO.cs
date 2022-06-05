using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Comments.DTOs
{
    public class CreateCommentDTO
    {

        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string Body { get; set; }
        
    }
}