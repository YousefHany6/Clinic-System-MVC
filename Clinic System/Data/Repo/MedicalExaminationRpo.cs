using ClosedXML.Excel;
using Core.IRepo;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Clinic_System.Data.Repo
{
	public class MedicalExaminationRpo : IMedicalExaminationRpo
	{
		private readonly ApplicationDbContext context;

		public MedicalExaminationRpo(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<AddModel> AddMedicalExamination(MedicalExaminationModel mems)
		{
			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				await context.MedicalExaminations.AddAsync(mems.MedicalExamination);
				await context.SaveChangesAsync();
				if (mems.MedicalExamination_X_ray != null)
				{
					mems.MedicalExamination.MedicalExamination_X_rays.AddRange(mems.MedicalExamination_X_ray);
				}
				if (mems.detectionTypes != null)
				{
					await context.MedicalExaminationDetectionTypes.AddRangeAsync(mems.detectionTypes.Select(d =>
					new MedicalExaminationDetectionType
					{
						DetectionTypeId = d,
						MedicalExaminationId = mems.MedicalExamination.MedicalExaminationId
					}

						)
						);
				}
				await context.SaveChangesAsync();
				await transaction.CommitAsync();

				return new AddModel { Confirm = true };
			}
		}

		public async Task<ICollection<MedicalExamination>> GetAllWithPatient()
		{
			var mems = await context.MedicalExaminations.
				Include(s => s.Patient)
				.Include(S => S.MedicalExamination_X_rays)
				.ToListAsync();
			return mems;
		}
		private async Task<ICollection<MedicalExamination>> MedicalExaminationsAll()
		{
			var mems = await context.MedicalExaminations.
				Include(s => s.Patient)
				.Include(S => S.MedicalExamination_X_rays)
				.Include(s => s.MDT)
				.ThenInclude(s => s.DetectionType)
				.ToListAsync();

			return mems;
		}

		public async Task<AddModel> Delete(int id)
		{
			var m = await context.MedicalExaminations.FindAsync(id);
			context.MedicalExaminations.Remove(m);
			await context.SaveChangesAsync();
			return new AddModel { Confirm = true };

		}

		public async Task<AddModel> Edit(MedicalExaminationModel mems)
		{
			var oldmed = await GetByidwithdetails(mems.MedicalExamination.MedicalExaminationId);
			context.Entry(oldmed.MedicalExamination).CurrentValues.SetValues(mems.MedicalExamination);
			if (mems.MedicalExamination_X_ray.Count != 0)
			{
				if (!mems.MedicalExamination_X_ray.Select(s => s.X_rayName)
			.SequenceEqual(oldmed.MedicalExamination.MedicalExamination_X_rays.Select(s => s.X_rayName)))
				{
					context.MedicalExamination_X_rays.RemoveRange(oldmed.MedicalExamination.MedicalExamination_X_rays);
					await context.MedicalExamination_X_rays.AddRangeAsync(mems.MedicalExamination_X_ray.Select(
						s => new MedicalExamination_X_ray
						{
							MedicalExaminationId = mems.MedicalExamination.MedicalExaminationId,
							X_rayName = s.X_rayName
						}
						));
				}
			}
			if (mems.detectionTypes.Count != 0)
			{
				context.MedicalExaminationDetectionTypes.RemoveRange(oldmed.MedicalExamination.MDT);
				await context.MedicalExaminationDetectionTypes.AddRangeAsync(mems.detectionTypes.Select(
					s => new MedicalExaminationDetectionType
					{
						DetectionTypeId = s,
						MedicalExaminationId = mems.MedicalExamination.MedicalExaminationId
					}
					));
			}
			await context.SaveChangesAsync();
			return new AddModel { Confirm = true };
		}
		public async Task<MedicalExaminationModel> GetByidwithdetails(int id)
		{
			var med = await context.MedicalExaminations
				.Include(s => s.Patient)
							.Include(m => m.MDT)
							.Include(s => s.MedicalExamination_X_rays)
							.FirstOrDefaultAsync(s => s.MedicalExaminationId == id);

			var detectionTypeIds = med.MDT.Select(mdt => mdt.DetectionTypeId).ToList();
			var detectionTypes = await context.DetectionTypes
							.Where(dt => detectionTypeIds.Contains(dt.DetectionTypeId))
							.ToListAsync();

			var mm = new MedicalExaminationModel
			{
				MedicalExamination = med,
				DTS = detectionTypes
			};

			return mm;
		}

		public async Task<ExportModel> ExportTOExcel()
		{
			var response = await MedicalExaminationsAll();
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
				var workSheet = workBook.Worksheets.Add("الكشوفات");
				int currentRow = 2;
				int index = 1;

				workSheet.Cell(currentRow, 2).Value = "سعر الكشف";
				workSheet.Cell(currentRow, 3).Value = "نوع الكشف";
				workSheet.Cell(currentRow, 4).Value = "الاشعة";
				workSheet.Cell(currentRow, 5).Value = "حالة المريض";
				workSheet.Cell(currentRow, 6).Value = "تاريخ اعاة الكشف";
				workSheet.Cell(currentRow, 7).Value = "تاريخ الكشف ";
				workSheet.Cell(currentRow, 8).Value = "ملاحظات";
				workSheet.Cell(currentRow, 9).Value = "اسم المريض";
				workSheet.Cell(currentRow, 10).Value = "#";
				//workSheet.Cell(currentRow, 11).Value = "#";

				foreach (var user in response)
				{
					currentRow++;
					workSheet.Cell(currentRow, 2).Value = user.DetectionPrice;
					workSheet.Cell(currentRow, 3).Value = user.MDT.Any() ? string.Join(",", user.MDT.Select(p => p.DetectionType.DetectionTypeName)) : "لا يوجد";
					workSheet.Cell(currentRow, 4).Value = user.MedicalExamination_X_rays.Any() ? string.Join(",", user.MedicalExamination_X_rays.Select(p => p.X_rayName)) : "لا يوجد";
					workSheet.Cell(currentRow, 5).Value = user.patientStatus;
					workSheet.Cell(currentRow, 6).Value = user.Re_ExaminationDate != null ? user.Re_ExaminationDate : "لا يوجد";
					workSheet.Cell(currentRow, 7).Value = user.DetectionDate;
					workSheet.Cell(currentRow, 8).Value = user.Notes != null ? user.Notes : "لا يوجد";
					workSheet.Cell(currentRow, 9).Value = user.Patient.PatientName;
					workSheet.Cell(currentRow, 10).Value = index++;
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
			var file = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = $"{date}_MedicalExamination.xlsx"
			};
			return new ExportModel
			{
				IsCorrectData = true,
				file = file
			};
		}


	}
}
