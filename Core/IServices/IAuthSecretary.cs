using Core.Models;
using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
	public interface IAuthSecretary
	{
		Task<ICollection<User>> GetAllSecretaryAsync();
		Task<AddModel> AddSecretary(User user, ICollection<UserPhoneNumber> userPhones);
		Task<bool> EditSecretary(User updatedUser, ICollection<UserPhoneNumber> userPhones);
		Task<bool> DeleteSecretary(string Id);
		Task<User> GetUserById(string Id);
		Task<User> GetUserByIdwithponeNumbers(string Id);
		Task<ExportModel> ExportSecs();

	}
}
