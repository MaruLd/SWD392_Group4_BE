using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
	public class UserFCMToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public virtual User? User { get; set; }
		public Guid? UserId { get; set; }

		public string Token { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}