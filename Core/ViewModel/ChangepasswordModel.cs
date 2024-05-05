using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
	public class ChangepasswordModel
	{
		[Required(ErrorMessage = "مطلوب")]
		public string currentpass { get; set; }
		[Required(ErrorMessage = "مطلوب")]
		public string Newpass { get; set; }
	}
}
