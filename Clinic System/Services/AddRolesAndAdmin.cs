using Core.Constant;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Clinic_System.Services
{
	public class AddRolesAndAdmin
	{
		public static async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

				await CreateRoles(roleManager);
				await AddSuperManager(userManager);
			}
		}

		private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
			{

				await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			}
			if (!await roleManager.RoleExistsAsync(Roles.Secretary.ToString()))
			{

				await roleManager.CreateAsync(new IdentityRole(Roles.Secretary.ToString()));
			}
		}

		private static async Task AddSuperManager(UserManager<User> userManager)
		{
			var superManager = await userManager.FindByEmailAsync(Admin.AdminEmail);

			if (superManager == null)
			{
				var user = new User
				{

					UserName = Admin.AdminEmail,
					Email = Admin.AdminEmail,
					EmailConfirmed = true,
					FullName = Admin.AdminFullName
				};

				var result = await userManager.CreateAsync(user, Admin.AdminPassword);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
				}
				else
				{
					throw new ApplicationException($"Unable to create super manager. Errors: {string.Join(",", result.Errors)}");
				}
			}
		}
	}
}
