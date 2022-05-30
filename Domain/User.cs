﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
	public string DisplayName { get; set; }

	// public int InventoryId { get; set; }
	[JsonIgnore]
	public virtual ICollection<Ticket> Tickets { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.Now;
}
