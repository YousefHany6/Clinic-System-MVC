using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
	public interface IMedicalExaminationRpo
	{
		Task<ExportModel> ExportTOExcel();
		Task<MedicalExaminationModel> GetByidwithdetails(int id);
		Task<ICollection<MedicalExamination>> GetAllWithPatient();
		Task<AddModel> AddMedicalExamination(MedicalExaminationModel mems);
		Task<AddModel> Delete(int id);
		Task<AddModel> Edit(MedicalExaminationModel mems);
	}
}
