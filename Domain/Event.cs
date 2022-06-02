using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain;

public class Event
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; set; }

	public String? Title { get; set; }
	public String? Description { get; set; }
	public String? ImageURL { get; set; }

	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }

	public float MultiplierFactor { get; set; }

	public virtual EventCategory? EventCategory { get; set; }
	public int? EventCategoryId { get; set; }

	[JsonIgnore]
	public virtual ICollection<Organizer> Organizers { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventUser> EventUser { get; set; }
	[JsonIgnore]
	public virtual ICollection<Ticket> Tickets { get; set; }
	[JsonIgnore]
	public virtual ICollection<Post> Posts { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventAgenda> EventAgenda { get; set; }

	public String Status { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.Now;

}
