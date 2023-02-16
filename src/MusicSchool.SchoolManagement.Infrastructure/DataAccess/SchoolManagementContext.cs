using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Infrastructure.DataAccess;

public class SchoolManagementContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SchoolManagementContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = _configuration.GetConnectionString("SchoolManagement");
        if (connectionString == null)
        {
            throw new InvalidOperationException("Missing connection string \"SchoolManagement\".");
        }

        optionsBuilder.UseMySQL(connectionString);
    }
}
