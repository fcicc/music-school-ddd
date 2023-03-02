using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Infrastructure.DataAccess.Configuration;

internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("enrollments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.HasOne(typeof(Student))
            .WithMany()
            .HasForeignKey(nameof(Enrollment.StudentId));

        builder.Property(e => e.CourseId).HasColumnName("course_id");
        builder.HasOne(typeof(Course))
            .WithMany()
            .HasForeignKey(nameof(Enrollment.CourseId));

        builder.Property(e => e.StartMonth).HasColumnName("start_month")
            .HasColumnType("char(7)")
            .HasConversion(
                m => m.ToString(),
                v => DateMonthOnly.Parse(v)
            );

        builder.Property(e => e.EndMonth).HasColumnName("end_month")
            .HasColumnType("char(7)")
            .HasConversion(
                m => m.ToString(),
                v => DateMonthOnly.Parse(v)
            );

        builder.Property(e => e.MonthlyBillingValue).HasColumnName("monthly_billing_value")
            .HasConversion(a => a.Value, v => new BrlAmount(v));
    }
}
