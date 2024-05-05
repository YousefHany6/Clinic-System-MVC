using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class UserPhoneNumber
	{
		[Key]
		public int Id { get; set; }
		public string? UserId { get; set; }
		[Required, Phone, MaxLength(11), MinLength(11)]
		public string PhoneNumber { get; set; }
		[ForeignKey("UserId")]
		public virtual User? user { get; set; }
	}
}
