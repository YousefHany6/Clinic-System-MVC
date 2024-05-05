using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
	public class MedicalExaminationModel
	{
		public MedicalExamination MedicalExamination { get; set; }
		public ICollection<MedicalExamination_X_ray>? MedicalExamination_X_ray { get; set; } = new HashSet<MedicalExamination_X_ray>();
		public ICollection<int> detectionTypes { get; set; } = new List<int>();
		public ICollection<DetectionType> DTS { get; set; } = new List<DetectionType>();
	}
}
