using Clinic_System.Data;
using Clinic_System.Services;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Clinic_System
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
							options.UseSqlServer(connectionString));

			builder.Services.AddDatabaseDeveloperPageExceptionFilter();
			builder.Services.Configure<RequestLocalizationOptions>(options =>
			{
				options.DefaultRequestCulture = new RequestCulture("ar"); // Set default culture to Arabic
				options.SupportedCultures = new List<CultureInfo> { new CultureInfo("ar") }; // Supported cultures
				options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("ar") }; // Supported UI cultures
			});
			builder.Services.AddIdentity<User, IdentityRole>()
							.AddEntityFrameworkStores<ApplicationDbContext>()
							.AddDefaultUI()
							.AddDefaultTokenProviders();
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<IAuthSecretary, AuthSecretary>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;


				options.Password.RequiredUniqueChars = 0;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequiredUniqueChars = 0;
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
							name: "default",
							pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			AddRolesAndAdmin.InitializeAsync(app.Services).Wait();

			app.Run();
		}
	}
}
