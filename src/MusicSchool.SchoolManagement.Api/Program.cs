using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.ValueObjects;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Json;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolManagementContext>(options =>
{
    options.UseMySQL(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("MusicSchool.SchoolManagement.Api")
    );
});

builder.Services.AddControllers()
    .AddJsonOptions(
        options => JsonConfigurationHelper.ConfigureJsonSerializerOptions(
            options.JsonSerializerOptions
        )
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<BrlAmount>(() => new OpenApiSchema
    {
        Type = "number",
    });
    options.MapType<DateMonthOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString(DateMonthOnly.Current.ToString()),
    });
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
    });
});

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
