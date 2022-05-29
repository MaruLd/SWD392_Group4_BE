using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain;

public class Post
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; set; }

	public string Title { get; set; }
	public string Content { get; set; }

	public virtual Event? Event { get; set; }
	public Guid? EventId { get; set; }

	public virtual User? User { get; set; }
	public Guid? UserId { get; set; }

	public String Status { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	[JsonIgnore]
	public virtual ICollection<Comment> Comments { get; set; }

}
