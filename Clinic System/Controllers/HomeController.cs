using Core.Constant;
using Core.IServices;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Admin")]

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
		private readonly UserManager<User> userManager;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<User> userManager)
		{
			_logger = logger;
			this.unitOfWork = unitOfWork;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);
			var roles = await userManager.GetRolesAsync(user);

			if (roles.Contains(Roles.Secretary.ToString()))
			{
				return RedirectToAction("GetAll", "Booking");
			}
			var cp = await unitOfWork.PatientRepo.GetAll();
			var cm = await unitOfWork.MedicalExamination.GetAll();
			var cb = await unitOfWork.BookingRpo.GetAllbooking();
			var cs = await userManager.GetUsersInRoleAsync(Roles.Secretary.ToString());

			return View(new Dashboardmodel
			{
				Countpa = cp.Count,
				Countme = cm.Count,
				Countbooking = cb.Count,
				CountSec = cs.Count,
				bookings = cb
			});
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExportToExcel()
		{
			var File = await unitOfWork.BookingRpo.ExportTOExcelSecs();
			if (!File.IsCorrectData)
			{
				ModelState.AddModelError(string.Empty, "لا يوجد بيانات");
				var cp = await unitOfWork.PatientRepo.GetAll();
				var cm = await unitOfWork.MedicalExamination.GetAll();
				var cb = await unitOfWork.BookingRpo.GetAllbooking();
				var cs = await userManager.GetUsersInRoleAsync(Roles.Secretary.ToString());

				return View("Index", new Dashboardmodel
				{
					Countpa = cp.Count,
					Countme = cm.Count,
					Countbooking = cb.Count,
					CountSec = cs.Count,
					bookings = cb
				}); ;
			}
			return File.file;
		}
		public async Task<IActionResult> ChangePasswordAdmin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePasswordAdmin(ChangepasswordModel changepassword)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var usr = await userManager.GetUserAsync(User);
			if (usr == null)
			{
				return NotFound();
			}
			var R = await userManager.ChangePasswordAsync(usr, changepassword.currentpass, changepassword.Newpass);

			if (!R.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "كلمة المرور الحالية غير صحيحة");
				return View(nameof(ChangePasswordAdmin));
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> InformationOfAdmin()
		{

			return View(await userManager.GetUserAsync(User));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> InformationOfAdmin(User user)
		{
			var olduser = await userManager.FindByIdAsync(user.Id);
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, "ادخل بيانات صحيحة");

				return View(olduser);
			}
			if (olduser.FullName.Trim().Equals(user.FullName.Trim()) && olduser.Email.Trim().Equals(user.Email.Trim()))
			{
				return RedirectToAction(nameof(Index));
			}
			olduser.FullName = user.FullName;
			olduser.Email = user.Email;
			olduser.UserName = user.Email;
			var r = await userManager.UpdateAsync(olduser);
			if (!r.Succeeded)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
