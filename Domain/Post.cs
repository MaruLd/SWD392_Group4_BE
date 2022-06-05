using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain;
public static class MyEnum
{
	public const string Foo = "Foo";
	public const string Bar = "Bar";
}

public class Post
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; set; }

	public string Title { get; set; }
	public string Content { get; set; }
	public string? ImageURL { get; set; }

	public virtual Event? Event { get; set; }
	public Guid? EventId { get; set; }

	public virtual User? User { get; set; }
	public Guid? UserId { get; set; }

	public StatusEnum Status { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.Now;

	[JsonIgnore]
	public virtual ICollection<Comment> Comments { get; set; }

}
