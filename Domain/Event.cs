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

public enum EventTriggerEnum
{
	Start, End
}

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
	public String? Location { get; set; }

	public virtual EventCategory? EventCategory { get; set; }
	public int? EventCategoryId { get; set; }


	[JsonIgnore]
	public virtual ICollection<Ticket> Tickets { get; set; }
	[JsonIgnore]
	public virtual ICollection<Post> Posts { get; set; }

	[JsonIgnore]
	public virtual ICollection<EventOrganizer> EventOrganizers { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventUser> EventUser { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventAgenda> EventAgenda { get; set; }

	public StatusEnum Status { get; set; }
	public EventStateEnum State { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.Now;

	// State Machine
	private StateMachine<int, int> _machine;

	public Event()
	{
		_machine = new StateMachine<int, int>((int)this.State);

		_machine.Configure((int)EventStateEnum.Idle)
			.Permit((int)EventTriggerEnum.Start, (int)EventStateEnum.Ongoing);

		_machine.Configure((int)EventStateEnum.Ongoing)
			.OnEntry(data => OnStart())
			.Permit((int)EventTriggerEnum.End, (int)EventStateEnum.Ended);

		_machine.Configure((int)EventStateEnum.Ended)
			.OnEntry(data => OnEnd());

		void OnStart()
		{
			this.State = EventStateEnum.Ongoing;
		}

		void OnEnd()
		{
			this.State = EventStateEnum.Ended;
		}
	}

	public void TriggerState(EventTriggerEnum eventTriggerEnum)
	{
		_machine.Fire((int)eventTriggerEnum);
	}

}