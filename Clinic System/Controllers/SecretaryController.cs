using Clinic_System.Services;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_System.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SecretaryController : Controller
	{
		private readonly IAuthSecretary _authSecretary;

		public SecretaryController(IAuthSecretary authSecretary)
		{
			_authSecretary = authSecretary;
		}
		[HttpGet]
		public async Task<IActionResult> AllSecretaries()
		{
			return View(await _authSecretary.GetAllSecretaryAsync());
		}
		[HttpGet]

		public async Task<IActionResult> AddSecretaryView()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddSecretary(User user, ICollection<UserPhoneNumber> phones)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			// Business logic to add a secretary
			var result = await _authSecretary.AddSecretary(user, phones);

			if (!result.Confirm)
			{
				ModelState.AddModelError(string.Empty, result.Message);
				return View(nameof(AddSecretaryView));
			}

			return RedirectToAction("AddSecretaryView");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteSecretary(string Id)
		{
			if (Id is null)
			{
				return BadRequest();
			}
			var result = await _authSecretary.DeleteSecretary(Id);
			if (!result)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(AllSecretaries));
		}
		[HttpGet]
		public async Task<IActionResult> EditSecretary(string Id)
		{
			return View(await _authSecretary.GetUserByIdwithponeNumbers(Id));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditSecretary(User user, ICollection<UserPhoneNumber> phones)
		{
			if (!ModelState.IsValid || user is null)
			{
				return BadRequest();
			}
			var r = await _authSecretary.EditSecretary(user, phones);
			if (!r)
			{
				return BadRequest();
			}
			return RedirectToAction(nameof(AllSecretaries));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExportToExcel()
		{
			var File = await _authSecretary.ExportSecs();
			if (!File.IsCorrectData)
			{
				ModelState.AddModelError(string.Empty, "لا يوجد بيانات");
				return View("AllSecretaries", await _authSecretary.GetAllSecretaryAsync());
			}
			return File.file;
		}

	}
}
