﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<Guid>
{
	public string DisplayName { get; set; }

	public string? ImageURL { get; set; }

	public int Bean { get; set; }

	// public int InventoryId { get; set; }
	[JsonIgnore]
	public virtual ICollection<TicketUser> TicketUsers { get; set; }
	[JsonIgnore]
	public virtual ICollection<CommentLike> CommentLikes { get; set; }
	[JsonIgnore]
	public virtual ICollection<UserImage> Images { get; set; }
	[JsonIgnore]
	public virtual ICollection<UserFCMToken> Tokens { get; set; }

	public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
}
