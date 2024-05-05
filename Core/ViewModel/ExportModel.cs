using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
	public class ExportModel
	{
		public FileStreamResult file { get; set; }

		public bool IsCorrectData { get; set; }
	}
}
