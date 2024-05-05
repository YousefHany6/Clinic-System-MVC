using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class User : IdentityUser
	{
		[Required(ErrorMessage = "الاسم مطلوب")
, MaxLength(150)]
		public string FullName { get; set; }
		[Required(ErrorMessage = "البريد الالكترونى مطلوب"), EmailAddress(ErrorMessage = "يرجى إدخال عنوان بريد إلكتروني صالح.")]
		public override string? Email { get => base.Email; set => base.Email = value; }
		public virtual ICollection<UserPhoneNumber> userPhoneNumbers { get; set; } = new HashSet<UserPhoneNumber>();
		public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}
