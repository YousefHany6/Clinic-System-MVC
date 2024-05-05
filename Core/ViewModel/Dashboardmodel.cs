using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
	public class Dashboardmodel
	{
		public int CountSec { get; set; }
		public int Countpa { get; set; }
		public int Countme { get; set; }
		public int Countbooking { get; set; }
		public ICollection<Booking> bookings { get; set; } = new List<Booking>();
	}
}
