using Core.IServices;
using Core.ViewModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Admin")]

	public class PatientController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment environment;

		public PatientController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
		{
			this.unitOfWork = unitOfWork;
			this.environment = environment;
		}
		public async Task<IActionResult> GetAllPatients()
		{
			return View(await unitOfWork.pp.GetALlPatientsWithDetails());
		}
		public async Task<IActionResult> AddPatientView()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddPatient(PatientModel Pmodel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await unitOfWork.pp.Addpatient(Pmodel);

			if (!result.Confirm)
			{
				return BadRequest();

			}
			return RedirectToAction(nameof(AddPatientView));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePatient(int Id)
		{
			await unitOfWork.PatientRepo.Delete(Id);
			await unitOfWork.Complete();
			return RedirectToAction(nameof(GetAllPatients));
		}
		public async Task<IActionResult> EditPatient(int Id)
		{


			return View(await unitOfWork.pp.GetPatientbyId(Id));
		}
		public async Task<IActionResult> PatientDetails(int Id)
		{


			return View(await unitOfWork.pp.GetPatientbyId(Id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditPatient(PatientModel Pmodel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(modelState: ModelState);
			}

			var r = await unitOfWork.pp.EditPatient(Pmodel);
			if (!r.Confirm)
			{
				return BadRequest();
			}
			return RedirectToAction(nameof(GetAllPatients));

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExportToExcel()
		{
			var File = await unitOfWork.pp.ExportTOExcel();
			if (!File.IsCorrectData)
			{
				ModelState.AddModelError(string.Empty, "لا يوجد بيانات");
				return View("GetAllPatients", await unitOfWork.pp.GetALlPatientsWithDetails());
			}
			return File.file;
		}

	}
}
