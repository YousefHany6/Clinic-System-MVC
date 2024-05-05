using Azure;
using Clinic_System.Services;
using ClosedXML.Excel;
using Core.IRepo;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Clinic_System.Data.Repo
{
	public class PatientRepo : IPatientRepo
	{
		private readonly ApplicationDbContext context;

		public PatientRepo(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<AddModel> Addpatient(PatientModel patient)
		{
			if (patient == null || patient.patient == null)
			{
				throw new ArgumentNullException(nameof(patient), "Patient or Patient data is null");
			}

			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				await context.AddAsync(patient.patient);
				await context.SaveChangesAsync();


				if (patient.phones != null)
				{
					patient.patient.patientPhoneNumbers.AddRange(patient.phones);
				}

				if (patient.patientChronicDiseases != null)
				{
					patient.patient.patientChronicDiseases.AddRange(patient.patientChronicDiseases);
				}

				if (patient.PatientAddresses != null)
				{
					patient.patient.PatientAddresses.Add(patient.PatientAddresses);
				}

				if (patient.patientSpecialCase != null)
				{
					patient.patient.patientSpecialCases.AddRange(patient.patientSpecialCase);
				}

				await context.SaveChangesAsync();
				await transaction.CommitAsync();
			}

			return new AddModel { Confirm = true };
		}


		public async Task<AddModel> EditPatient(PatientModel patient)
		{
			if (patient == null || patient.patient == null)
			{
				throw new ArgumentNullException(nameof(patient), "Patient or Patient data is null");
			}

			var oldpatient = await GetPatientbyId(patient.patient.patientId);

			context.Entry(oldpatient.patient).CurrentValues.SetValues(patient.patient);

			if (patient.patientSpecialCase.Count != 0)
			{
				if (!patient.patientSpecialCase.Select(s => s.SpecialCase)
				.SequenceEqual(oldpatient.patient.patientSpecialCases.Select(s => s.SpecialCase)))
				{
					context.patientSpecialCases.RemoveRange(oldpatient.patient.patientSpecialCases);
					await context.patientSpecialCases.AddRangeAsync(patient.patientSpecialCase
						.Select(s =>
						new patientSpecialCase
						{
							SpecialCase = s.SpecialCase,
							patientId = patient.patient.patientId
						}
						)
						);
				}

			}

			if (!patient.phones.Select(s => s.PhoneNumber)
							.SequenceEqual(oldpatient.patient.patientPhoneNumbers.Select(s => s.PhoneNumber)))
			{
				context.RemoveRange(oldpatient.patient.patientPhoneNumbers);

				await context.patientPhoneNumbers.AddRangeAsync(patient.phones.Select(s => new patientPhoneNumber
				{
					PhoneNumber = s.PhoneNumber,
					patientId = patient.patient.patientId
				}
				)
					);

			}
			if (patient.patientChronicDiseases.Count != 0)
			{
				if (!patient.patientChronicDiseases.Select(s => s.ChronicDisease)
					.SequenceEqual(oldpatient.patient.patientChronicDiseases.Select(s => s.ChronicDisease)))
				{
					context.RemoveRange(oldpatient.patient.patientChronicDiseases);
					await context.patientChronicDiseases.AddRangeAsync(patient.patientChronicDiseases
						.Select(s =>
						new patientChronicDisease
						{
							ChronicDisease = s.ChronicDisease,
							patientId = patient.patient.patientId
						}
						)
						);
				}

			}

			if (!patient.PatientAddresses.Address.Equals(oldpatient.PatientAddresses.Address))
			{
				context.Remove(oldpatient.patient.PatientAddresses.FirstOrDefault());
				patient.patient.PatientAddresses.Add(patient.PatientAddresses);
			}

			await context.SaveChangesAsync();

			return new AddModel { Confirm = true };
		}

		public async Task<ExportModel> ExportTOExcel()
		{
			var response = await Patients();
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
				var workSheet = workBook.Worksheets.Add("المرضى");
				int currentRow = 2;
				int index = 1;

				workSheet.Cell(currentRow, 3).Value = "الحالة الخاصة";
				workSheet.Cell(currentRow, 4).Value = "الامراض المزمنة";
				workSheet.Cell(currentRow, 5).Value = "الهاتف";
				workSheet.Cell(currentRow, 6).Value = "ملاحظات عن المريض";
				workSheet.Cell(currentRow, 7).Value = "عدد الكشوفات";
				workSheet.Cell(currentRow, 8).Value = "العمر";
				workSheet.Cell(currentRow, 9).Value = "جنس المريض";
				workSheet.Cell(currentRow, 10).Value = "اسم المريض";
				workSheet.Cell(currentRow, 11).Value = "#";

				foreach (var user in response)
				{
					currentRow++;
					workSheet.Cell(currentRow, 3).Value = user.patientSpecialCases.Any() ? string.Join(",", user.patientSpecialCases.Select(p => p.SpecialCase)) : "لا يوجد";
					workSheet.Cell(currentRow, 4).Value = user.patientChronicDiseases.Any() ? string.Join(",", user.patientChronicDiseases.Select(p => p.ChronicDisease)) : "لا يوجد";
					workSheet.Cell(currentRow, 5).Value = user.patientPhoneNumbers.Any() ? string.Join(",", user.patientPhoneNumbers.Select(p => p.PhoneNumber)) : "لا يوجد";
					workSheet.Cell(currentRow, 6).Value = user.Notes != null ? user.Notes : "لا يوجد";
					workSheet.Cell(currentRow, 7).Value = user.MedicalExamination.Count;
					workSheet.Cell(currentRow, 8).Value = user.Age;
					workSheet.Cell(currentRow, 9).Value = user.Gender.ToString();
					workSheet.Cell(currentRow, 10).Value = user.PatientName;
					workSheet.Cell(currentRow, 11).Value = index++;
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
				FileDownloadName = $"{date}_Patients.xlsx"
			};
			return new ExportModel
			{
				IsCorrectData = true,
				file = File
			};
		}

		public async Task<ICollection<patient>> Patients()
		{
			var patients = await context.patient
				.Include(s => s.MedicalExamination)
				.Include(s => s.patientPhoneNumbers)
				.Include(s => s.patientChronicDiseases)
				.Include(s => s.patientSpecialCases)
				.ToListAsync();
			return patients;
		}
		public async Task<ICollection<patient>> GetALlPatientsWithDetails()
		{
			var patients = await context.patient
				.Include(s => s.MedicalExamination)
				.Include(s => s.patientPhoneNumbers)
				.ToListAsync();
			return patients;
		}

		public async Task<PatientModel> GetPatientbyId(int id)
		{
			var patient = await context.patient
																				.AsNoTracking()
																				.Include(s => s.patientChronicDiseases)
																				.Include(s => s.patientSpecialCases)
																				.Include(s => s.patientPhoneNumbers)
																				.Include(s => s.PatientAddresses)
																				.Include(s => s.MedicalExamination)
																				.ThenInclude(s => s.MDT)
																				.ThenInclude(s => s.DetectionType)
																				.Include(s => s.MedicalExamination)
																				.ThenInclude(s => s.MedicalExamination_X_rays)
																				.FirstOrDefaultAsync(s => s.patientId == id);

			var patientModel = new PatientModel
			{
				patient = patient
				,
				PatientAddresses = patient.PatientAddresses.FirstOrDefault()

			};

			return patientModel;
		}

	}
}
