using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class MedicalExamination
	{
		[Key]
		public int MedicalExaminationId { get; set; }
		public string? Notes { get; set; }

		public DateTime DetectionDate { get; set; } = DateTime.Now;
		public DateTime? Re_ExaminationDate { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public decimal DetectionPrice { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string patientStatus { get; set; }
		public int patientId { get; set; }
		[ForeignKey("patientId")]
		public patient? Patient { get; set; }

		public virtual ICollection<MedicalExamination_X_ray> MedicalExamination_X_rays { get; set; } = new HashSet<MedicalExamination_X_ray>();
		public virtual ICollection<MedicalExaminationDetectionType> MDT { get; set; } = new HashSet<MedicalExaminationDetectionType>();

	}
}
