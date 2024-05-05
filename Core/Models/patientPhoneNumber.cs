using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class patientPhoneNumber
	{
		[Key]
		public int Id { get; set; }
		public int patientId { get; set; }
		[Required(ErrorMessage = "مطلوب"), Phone, MaxLength(11), MinLength(11)]
		public string PhoneNumber { get; set; }
		[ForeignKey(nameof(patientId))]
		public virtual patient? Patient { get; set; }
	}
}
