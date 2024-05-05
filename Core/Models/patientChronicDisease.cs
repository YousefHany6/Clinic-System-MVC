using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class patientChronicDisease
	{
		[Key]
		public int Id { get; set; }
		public int patientId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string ChronicDisease { get; set; }
		[ForeignKey(nameof(patientId))]
		public virtual patient? Patient { get; set; }
	}
}
