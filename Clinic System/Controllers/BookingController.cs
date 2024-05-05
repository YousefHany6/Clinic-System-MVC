using Clinic_System.Services;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Secretary")]
	public class BookingController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<User> userManager;

		public BookingController(IUnitOfWork unitOfWork, UserManager<User> userManager)
		{
			_unitOfWork = unitOfWork;
			this.userManager = userManager;
		}
		public async Task<IActionResult> GetAll()
		{

			return View(await _unitOfWork.BookingRpo.GetAll());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int Id)
		{
			await _unitOfWork.BookingRpo.Delete(Id);
			await _unitOfWork.Complete();

			return RedirectToAction(nameof(GetAll));

		}
		public async Task<IActionResult> Edit(int Id)
		{
			return View(await _unitOfWork.BookingRpo.Getbyid(Id));

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Booking booking)
		{
			var u = await userManager.GetUserAsync(User);
			if (u == null)
			{
				return BadRequest();
			}
			booking.UserId = u.Id;
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _unitOfWork.BookingRpo.Add(booking);
			await _unitOfWork.Complete();
			return RedirectToAction(nameof(GetAll));

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Booking booking)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _unitOfWork.BookingRpo.Edit(booking);
			await _unitOfWork.Complete();

			return RedirectToAction(nameof(GetAll));

		}
	}
}
