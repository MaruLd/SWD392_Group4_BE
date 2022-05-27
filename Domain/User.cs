using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<int>
{
	public DateTime Date { get; set; }

	public virtual ICollection<Ticket> Tickets { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.Now;
}
