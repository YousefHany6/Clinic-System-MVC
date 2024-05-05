using Core.Models;
using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
	public interface IRepo<T> where T : class
	{
		Task<ICollection<Booking>> GetAllbooking();
		Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
		Task<ICollection<T>> GetAll();
		Task<T> Getbyid(int id);
		Task Add(T model);
		Task Edit(T model);
		Task Delete(int id);
		Task<List<DetectionType>> countDetectionType(List<int> detectionTypeIds);
		Task<ExportModel> ExportTOExcelSecs();
	}
}
