using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Api.SwaggerGen;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolManagementContext>(options =>
{
    options.UseMySQL(
        builder.Configuration.GetConnectionString("Default") ?? "",
        b => b.MigrationsAssembly("MusicSchool.SchoolManagement.Api")
    );
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGenHelper.Setup);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
