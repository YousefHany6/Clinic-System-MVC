using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class patientAddress
	{
		[Key]
		public int Id { get; set; }
		public int patientId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string Address { get; set; }
		[ForeignKey(nameof(patientId))]
		public virtual patient? Patient { get; set; }
	}
}
