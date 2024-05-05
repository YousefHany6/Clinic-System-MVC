using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class Booking
	{
		[Key]
		public int BookingId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string BookingName { get; set; }
		[Required(ErrorMessage = "مطلوب"), DataType(dataType: DataType.DateTime)]
		public DateTime BookingDate { get; set; }
		public string? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User? user { get; set; }


	}
}
