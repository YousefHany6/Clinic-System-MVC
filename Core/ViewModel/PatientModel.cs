using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
	public class PatientModel
	{
		public patient patient { get; set; }
		public patientAddress PatientAddresses { get; set; }
		public ICollection<patientPhoneNumber> phones { get; set; } = new HashSet<patientPhoneNumber>();
		public ICollection<patientChronicDisease>? patientChronicDiseases { get; set; } = new HashSet<patientChronicDisease>();
		public ICollection<patientSpecialCase>? patientSpecialCase { get; set; } = new HashSet<patientSpecialCase>();
		public ICollection<DetectionType> DetectionTypes { get; set; } = new HashSet<DetectionType>();




	}
}
