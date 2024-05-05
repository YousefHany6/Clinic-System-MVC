using ClosedXML.Excel;
using Core.IRepo;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clinic_System.Data.Repo
{
	public class Repo<T> : IRepo<T> where T : class
	{
		private readonly ApplicationDbContext dBContext;

		public Repo(ApplicationDbContext dBContext)
		{
			this.dBContext = dBContext;
		}
		public async Task<ICollection<Booking>> GetAllbooking()
		{
			return await dBContext.Set<Booking>().Include(s => s.user).ToListAsync();
		}
		public async Task<ICollection<T>> GetAll()
		{
			return await dBContext.Set<T>().ToListAsync();
		}

		public async Task<T> Getbyid(int id)
		{
			return await dBContext.Set<T>().FindAsync(id);
		}
		public async Task Add(T model)
		{

			await dBContext.Set<T>().AddAsync(model);

		}
		public async Task Delete(int id)
		{
			var model = await Getbyid(id);
			dBContext.Set<T>().Remove(model);

		}
		public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = dBContext.Set<T>();

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return await query.SingleOrDefaultAsync(criteria);
		}
		public async Task Edit(T model)
		{
			dBContext.Set<T>().Update(model);
		}
		public async Task<List<DetectionType>> countDetectionType(List<int> detectionTypeIds)
		{
			var dt = await dBContext.DetectionTypes
								.Where(dt => detectionTypeIds.Contains(dt.DetectionTypeId))
								.ToListAsync();
			return dt;
		}

		public async Task<ExportModel> ExportTOExcelSecs()
		{
			var response = await GetAllbooking();
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
				var workSheet = workBook.Worksheets.Add("الحجوات");
				int currentRow = 2;
				int index = 1;

				workSheet.Cell(currentRow, 3).Value = "حالة الحجز";
				workSheet.Cell(currentRow, 4).Value = "تاريخ الحجز";
				workSheet.Cell(currentRow, 5).Value = "اسم الحجز";
				workSheet.Cell(currentRow, 6).Value = "اسم الموظف";
				workSheet.Cell(currentRow, 7).Value = "#";

				foreach (var user in response)
				{
					currentRow++;
					workSheet.Cell(currentRow, 3).Value = user.BookingDate > DateTime.Now ? "فعال" : "انتهى";
					workSheet.Cell(currentRow, 4).Value = user.BookingDate.ToString("yyyy-MM-dd"); ;
					workSheet.Cell(currentRow, 5).Value = user.BookingName;
					workSheet.Cell(currentRow, 6).Value = user.user.FullName;
					workSheet.Cell(currentRow, 7).Value = index++;
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
				FileDownloadName = $"{date}_Booking.xlsx"
			};
			return new ExportModel
			{
				IsCorrectData = true,
				file = File
			};
		}

	}
}
