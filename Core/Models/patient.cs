using Core.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class patient
	{
		[Key]
		public int patientId { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string PatientName { get; set; }
		public string? Notes { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public int Age { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public Gender Gender { get; set; }
		public virtual ICollection<patientPhoneNumber> patientPhoneNumbers { get; set; } = new HashSet<patientPhoneNumber>();
		public virtual ICollection<patientAddress> PatientAddresses { get; set; } = new HashSet<patientAddress>();
		public virtual ICollection<patientSpecialCase> patientSpecialCases { get; set; } = new HashSet<patientSpecialCase>();
		public virtual ICollection<patientChronicDisease> patientChronicDiseases { get; set; } = new HashSet<patientChronicDisease>();
		public virtual ICollection<MedicalExamination> MedicalExamination { get; set; } = new HashSet<MedicalExamination>();

	}
}
