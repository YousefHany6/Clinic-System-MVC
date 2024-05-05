using Clinic_System.Data.Repo;
using Clinic_System.Data;
using Core.IRepo;
using Core.IServices;
using Core.Models;

namespace Clinic_System.Services
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public IRepo<DetectionType> DetectionTypeRepo { get; private set; }
		public IRepo<patient> PatientRepo { get; private set; }
		public IRepo<Booking> BookingRpo { get; private set; }
		public IRepo<MedicalExamination> MedicalExamination { get; private set; }
		public IPatientRepo pp { get; private set; }
		public IMedicalExaminationRpo MR { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			DetectionTypeRepo = new Repo<DetectionType>(context);
			PatientRepo = new Repo<patient>(context);
			pp = new PatientRepo(context);
			MR = new MedicalExaminationRpo(context);
			BookingRpo = new Repo<Booking>(context);
			MedicalExamination = new Repo<MedicalExamination>(context);
		}

		public async Task<int> Complete()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
