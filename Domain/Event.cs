using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enums;
using Stateless;

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
	public string? Location { get; set; }

	public virtual EventCategory? EventCategory { get; set; }
	public int? EventCategoryId { get; set; }


	[JsonIgnore]
	public virtual ICollection<Ticket> Tickets { get; set; }
	[JsonIgnore]
	public virtual ICollection<Post> Posts { get; set; }

	[JsonIgnore]
	public virtual ICollection<EventOrganizer> EventOrganizers { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventUser> EventUsers { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventAgenda> EventAgendas { get; set; }

	public StatusEnum Status { get; set; }
	public EventStateEnum State { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.Now;
}