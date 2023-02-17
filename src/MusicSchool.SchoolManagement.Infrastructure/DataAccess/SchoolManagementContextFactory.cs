using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MusicSchool.SchoolManagement.Infrastructure.DataAccess;

public class SchoolManagementContextFactory : IDesignTimeDbContextFactory<SchoolManagementContext>
{
    public SchoolManagementContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<SchoolManagementContext> optionsBuilder = new();
        optionsBuilder.UseMySQL("server=localhost; database=school_management; user=root; password=password");

        return new SchoolManagementContext(optionsBuilder.Options);
    }
}
