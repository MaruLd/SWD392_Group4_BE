using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain;

public class Event
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public String? Title { get; set; }
	public String? Description { get; set; }

	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }

	public float Multiplier_Factor { get; set; }

	public virtual EventCategory? Category { get; set; }
	public int? EventCategoryId { get; set; }

	public virtual ICollection<Organizer> Organizers { get; set; }
	public virtual ICollection<Participant> Participants { get; set; }
	public virtual ICollection<Ticket> Tickets { get; set; }
	public virtual ICollection<Post> Posts { get; set; }
	public virtual ICollection<EventAgenda> EventAgenda { get; set; }

	public String Status { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.Now;
}
