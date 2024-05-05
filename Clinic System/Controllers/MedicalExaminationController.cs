using Core.IServices;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Admin")]

	public class MedicalExaminationController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public MedicalExaminationController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		public async Task<IActionResult> GetAll()
		{
			return View(await unitOfWork.MR.GetAllWithPatient());
		}

		public async Task<IActionResult> AddView()
		{
			ViewBag.patients = new SelectList(await unitOfWork.PatientRepo.GetAll(), "patientId", "PatientName");
			ViewBag.DetectionTypes = new SelectList(await unitOfWork.DetectionTypeRepo.GetAll(), "DetectionTypeId", "DetectionTypeName");
			return View();
		}
		public async Task<IActionResult> EditView(int Id)
		{
			var mwithp = await unitOfWork.MR.GetByidwithdetails(Id);
			ViewBag.patients = new SelectList(await unitOfWork.PatientRepo.GetAll(), "patientId", "PatientName", mwithp.DTS);
			ViewBag.DetectionTypes = new SelectList(await unitOfWork.DetectionTypeRepo.GetAll(), "DetectionTypeId", "DetectionTypeName", mwithp.MedicalExamination.MDT);

			return View(mwithp);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(MedicalExaminationModel mem)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var r = await unitOfWork.MR.Edit(mem);
			if (!r.Confirm)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(GetAll));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(MedicalExaminationModel mem)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var r = await unitOfWork.MR.AddMedicalExamination(mem);
			if (!r.Confirm)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(AddView));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int Id)
		{
			if (Id == 0)
			{
				return BadRequest();
			}
			var r = await unitOfWork.MR.Delete(Id);
			if (!r.Confirm)
			{
				return BadRequest();
			}
			return RedirectToAction(nameof(GetAll));
		}
		public async Task<IActionResult> MedicalExaminationDetails(int Id)
		{
			return View(await unitOfWork.MR.GetByidwithdetails(Id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExportToExcel()
		{
			var file = await unitOfWork.MR.ExportTOExcel();
			if (!file.IsCorrectData)
			{
				ModelState.AddModelError(string.Empty, "لا يوجد بيانات");
				return View("GetAll", await unitOfWork.MR.GetAllWithPatient());
			}
			return file.file;
		}
	}
}
