using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolManagementContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
