using Clinic_System.Data;
using ClosedXML.Excel;
using Core.Constant;
using Core.IServices;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Clinic_System.Services
{
	public class AuthSecretary : IAuthSecretary
	{
		private readonly UserManager<User> userManager;
		private readonly ApplicationDbContext context;

		public AuthSecretary(UserManager<User> userManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.context = context;
		}
		public async Task<AddModel> AddSecretary(User user, ICollection<UserPhoneNumber> userPhones)
		{
			var userFound = await userManager.FindByEmailAsync(user.Email);
			if (userFound != null)
			{
				return new AddModel
				{
					Message = $"البريد الالكترونى {user.Email} موجود بالفعل"
				};
			}
			user.UserName = user.Email;
			user.EmailConfirmed = true;
			using (var transaction = await context.Database.BeginTransactionAsync())
			{
				var createResult = await userManager.CreateAsync(user, user.PasswordHash);
				if (!createResult.Succeeded)
				{
					await transaction.RollbackAsync();
					return new AddModel { Confirm = false };

				}

				var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Secretary.ToString());
				if (!addToRoleResult.Succeeded)
				{
					await transaction.RollbackAsync();
					return new AddModel { Confirm = false };
				}
				context.UserPhoneNumbers.AddRange(userPhones.Select(phone => new UserPhoneNumber
				{
					UserId = user.Id,
					PhoneNumber = phone.PhoneNumber
				}));

				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}

			return new AddModel { Confirm = true };
		}

		public async Task<bool> DeleteSecretary(string Id)
		{
			var userfound = await GetUserById(Id);
			if (userfound == null)
			{
				return false;
			}
			var result = await userManager.DeleteAsync(userfound);
			if (!result.Succeeded)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> EditSecretary(User updatedUser, ICollection<UserPhoneNumber> userPhones)
		{
			var existingUser = await GetUserByIdwithponeNumbers(updatedUser.Id);

			if (existingUser == null)
			{
				throw new KeyNotFoundException($"User with ID {updatedUser.Id} not found.");
			}

			existingUser.FullName = updatedUser.FullName;
			existingUser.Email = updatedUser.Email;
			existingUser.UserName = updatedUser.Email;
			if (updatedUser.PasswordHash != null)
			{
				existingUser.PasswordHash = updatedUser.PasswordHash;
			}
			if (existingUser.userPhoneNumbers != userPhones)
			{
				context.RemoveRange(existingUser.userPhoneNumbers);
				existingUser.userPhoneNumbers.AddRange(userPhones);
			}
			var result = await userManager.UpdateAsync(existingUser);

			return result.Succeeded;
		}


		public async Task<ICollection<User>> GetAllSecretaryAsync()
		{
			var secretariesWithPhones = await (
													from user in context.Users
													join userRole in context.UserRoles on user.Id equals userRole.UserId
													join role in context.Roles on userRole.RoleId equals role.Id
													where role.Name == Roles.Secretary.ToString()
													join phone in context.UserPhoneNumbers on user.Id equals phone.UserId into phoneGroup
													select new User
													{
														Id = user.Id,
														UserName = user.UserName,
														Email = user.Email,
														FullName = user.FullName,
														userPhoneNumbers = phoneGroup.ToList()
													}
									).ToListAsync();

			return secretariesWithPhones;
		}

		public async Task<User> GetUserById(string Id)
		{
			var userfound = await userManager.FindByIdAsync(Id);

			return userfound;

		}
		public async Task<User> GetUserByIdwithponeNumbers(string Id)
		{
			var userfound = await context.Users
				.Include(s => s.userPhoneNumbers)
				.FirstOrDefaultAsync(s => s.Id == Id);

			return userfound;

		}

		public async Task<ExportModel> ExportSecs()
		{
			var response = await GetAllSecretaryAsync();
			if (!response.Any())
			{
				return new ExportModel
				{
					IsCorrectData = false
				};
			}
			var stream = new MemoryStream();
			using (var workBook = new XLWorkbook())
			{
				var workSheet = workBook.Worksheets.Add("الموظفين");
				int currentRow = 2;
				int index = 1;

				workSheet.Cell(currentRow, 3).Value = "الهاتف";
				workSheet.Cell(currentRow, 4).Value = "البريد ألالكترونى";
				workSheet.Cell(currentRow, 5).Value = "اسم الموظف";
				workSheet.Cell(currentRow, 6).Value = "#";

				foreach (var user in response)
				{
					currentRow++;
					workSheet.Cell(currentRow, 3).Value = user.userPhoneNumbers.Any() ? string.Join(",", user.userPhoneNumbers.Select(p => p.PhoneNumber)) : "لا يوجد";
					workSheet.Cell(currentRow, 4).Value = user.Email;
					workSheet.Cell(currentRow, 5).Value = user.FullName;
					workSheet.Cell(currentRow, 6).Value = index++;
				}
				workSheet.RangeUsed().Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				workSheet.Columns().Width = 25;
				var table = workSheet.RangeUsed().CreateTable("Patient Table");
				table.Theme = XLTableTheme.TableStyleMedium9;
				table.SetShowHeaderRow(true);
				table.SetShowAutoFilter(true);
				workBook.SaveAs(stream);
			}
			stream.Position = 0;
			var date = DateTime.Now;

			var File = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = $"{date}_Secretaries.xlsx"
			};
			return new ExportModel
			{
				IsCorrectData = true,
				file = File
			};
		}



	}
}
