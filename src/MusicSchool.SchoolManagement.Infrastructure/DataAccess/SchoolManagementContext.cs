using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess.Configuration;

namespace MusicSchool.SchoolManagement.Infrastructure.DataAccess;

public class SchoolManagementContext : DbContext
{
    public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new CourseConfiguration())
            .ApplyConfiguration(new EnrollmentConfiguration())
            .ApplyConfiguration(new StudentConfiguration());
    }
}
