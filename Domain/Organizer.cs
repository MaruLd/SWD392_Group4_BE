using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain
{
	public class Organizer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public String? Name { get; set; }
		public String? Description { get; set; }
		public String? ImageURL { get; set; }

		public StatusEnum Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		[JsonIgnore]
		public virtual ICollection<EventOrganizer> EventOrganizers { get; set; }
	}
}