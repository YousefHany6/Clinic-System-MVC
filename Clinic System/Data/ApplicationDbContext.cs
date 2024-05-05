using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions options)
						: base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<MedicalExaminationDetectionType>()
				.HasKey(k => new { k.DetectionTypeId, k.MedicalExaminationId });

			builder.Entity<User>()
				.HasMany(s => s.Bookings)
				.WithOne(s => s.user)
				.HasForeignKey(s => s.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Entity<User>()
			.HasMany(s => s.userPhoneNumbers)
			.WithOne(s => s.user)
			.HasForeignKey(s => s.UserId)
			.OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(builder);
		}

		public virtual DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }
		public virtual DbSet<Booking> Bookings { get; set; }
		public virtual DbSet<DetectionType> DetectionTypes { get; set; }
		public virtual DbSet<MedicalExamination> MedicalExaminations { get; set; }
		public virtual DbSet<MedicalExamination_X_ray> MedicalExamination_X_rays { get; set; }
		public virtual DbSet<MedicalExaminationDetectionType> MedicalExaminationDetectionTypes { get; set; }
		public virtual DbSet<patient> patient { get; set; }
		public virtual DbSet<patientPhoneNumber> patientPhoneNumbers { get; set; }
		public virtual DbSet<patientAddress> patientAdresses { get; set; }
		public virtual DbSet<patientSpecialCase> patientSpecialCases { get; set; }
		public virtual DbSet<patientChronicDisease> patientChronicDiseases { get; set; }
	}
}
