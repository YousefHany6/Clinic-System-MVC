using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic_System.Data;
using Core.Models;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Core.Constant;

namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DetectionTypeController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public DetectionTypeController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		// GET: DetectionType
		public async Task<IActionResult> GetAll()
		{
			return View(await unitOfWork.DetectionTypeRepo.GetAll());
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("DetectionTypeId,DetectionTypeName,DetectionTypePrice")] DetectionType detectionType)
		{
			if (ModelState.IsValid)
			{
				await unitOfWork.DetectionTypeRepo.Add(detectionType);
				await unitOfWork.Complete();
				return RedirectToAction(nameof(Create));
			}
			return View(detectionType);
		}

		// GET: DetectionType/Edit/5
		public async Task<IActionResult> Edit(int Id)
		{
			if (Id == 0)
			{
				return NotFound();
			}

			var detectionType = await unitOfWork.DetectionTypeRepo.Getbyid(Id);
			if (detectionType == null)
			{
				return NotFound();
			}
			return View(detectionType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("DetectionTypeId,DetectionTypeName,DetectionTypePrice")] DetectionType detectionType)
		{
			if (id != detectionType.DetectionTypeId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{

				await unitOfWork.DetectionTypeRepo.Edit(detectionType);
				await unitOfWork.Complete();

				return RedirectToAction(nameof(GetAll));
			}
			return View(detectionType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int Id)
		{
			await unitOfWork.DetectionTypeRepo.Delete(Id);

			await unitOfWork.Complete();
			return RedirectToAction(nameof(GetAll));
		}

		[HttpPost]
		public async Task<IActionResult> CalculateTotalPrice(List<int> detectionTypeIds)
		{
			if (detectionTypeIds == null || detectionTypeIds.Count == 0)
			{
				return Json(new { totalPrice = 0 });
			}

			var totalPrice = 0m;
			var detectionTypes = await unitOfWork.DetectionTypeRepo.countDetectionType(detectionTypeIds);

			foreach (var detectionType in detectionTypes)
			{
				totalPrice += detectionType.DetectionTypePrice;
			}

			return Json(new { totalPrice });
		}


	}
}
