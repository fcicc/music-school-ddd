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

        builder.Property(e => e.StartDate).HasColumnName("start_date")
            .HasColumnType("date")
            .HasConversion(
                d => new DateTime(d.Year, d.Month, d.Day),
                d => new DateOnly(d.Year, d.Month, d.Day)
            );

        builder.Property(e => e.EndDate).HasColumnName("end_date")
            .HasColumnType("date")
            .HasConversion(
                d => new DateTime(d.Year, d.Month, d.Day),
                d => new DateOnly(d.Year, d.Month, d.Day)
            );

        builder.Property(e => e.MonthlyBill).HasColumnName("monthly_bill")
            .HasConversion(a => (decimal)a, v => (BrlAmount)v);
    }
}
