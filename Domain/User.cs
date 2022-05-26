using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public DateTime Date { get; set; }

	public virtual ICollection<Ticket> Tickets { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.Now;
}
