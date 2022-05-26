using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain;

public class Post
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; set; }

	public string Title { get; set; }
	public string Content { get; set; }

	[ForeignKey("Event")]
	public virtual Event? Event { get; set; }
	public String? EventId { get; set; }

	[ForeignKey("User")]
	public virtual User? User { get; set; }
	public String? UserId { get; set; }

	public virtual ICollection<Comment> Comments { get; set; }

}
