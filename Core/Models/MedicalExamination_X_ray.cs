using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

	public class MedicalExamination_X_ray
	{
		[Key]
		public int Id { get; set; }
		public int MedicalExaminationId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string X_rayName { get; set; }
		[ForeignKey("MedicalExaminationId")]
		public virtual MedicalExamination? MedicalExamination { get; set; }

	}
}
