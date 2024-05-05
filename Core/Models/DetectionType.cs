using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class DetectionType
	{
		[Key]
		public int DetectionTypeId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string DetectionTypeName { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public decimal DetectionTypePrice { get; set; }
		public virtual ICollection<MedicalExaminationDetectionType> MDT { get; set; } = new HashSet<MedicalExaminationDetectionType>();

	}
}
