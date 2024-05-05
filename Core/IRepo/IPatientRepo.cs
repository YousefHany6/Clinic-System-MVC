using Core.Models;
using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core.IRepo
{
	public interface IPatientRepo
	{
		Task<ExportModel> ExportTOExcel();
		Task<ICollection<patient>> GetALlPatientsWithDetails();
		Task<PatientModel> GetPatientbyId(int id);
		Task<AddModel> Addpatient(PatientModel patient);
		Task<AddModel> EditPatient(PatientModel patient);
		Task<ICollection<patient>> Patients();
	}
}
