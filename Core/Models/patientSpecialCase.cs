using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class patientSpecialCase
	{
		[Key]
		public int Id { get; set; }
		public int patientId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string SpecialCase { get; set; }
		[ForeignKey(nameof(patientId))]
		public virtual patient? Patient { get; set; }
	}
}
