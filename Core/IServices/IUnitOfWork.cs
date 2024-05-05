using Core.IRepo;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
	public interface IUnitOfWork : IDisposable
	{
		IRepo<DetectionType> DetectionTypeRepo { get; }
		IRepo<Booking> BookingRpo { get; }
		IRepo<patient> PatientRepo { get; }
		IRepo<MedicalExamination> MedicalExamination { get; }
		IPatientRepo pp { get; }
		IMedicalExaminationRpo MR { get; }
		Task<int> Complete();
	}
}
