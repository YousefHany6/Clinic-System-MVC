using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class MedicalExaminationDetectionType
	{
		public int DetectionTypeId { get; set; }
		public int MedicalExaminationId { get; set; }
		[ForeignKey(nameof(MedicalExaminationId))]
		public virtual MedicalExamination? MedicalExamination { get; set; }
		[ForeignKey(nameof(DetectionTypeId))]
		public virtual DetectionType? DetectionType { get; set; }


	}
}
