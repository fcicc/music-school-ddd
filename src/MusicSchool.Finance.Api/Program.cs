using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;
using MusicSchool.Finance.Infrastructure.DataAccess;
using MusicSchool.Finance.Infrastructure.External.SchoolManagement;
using MusicSchool.Finance.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FinanceContext>(options =>
{
    options.UseMySQL(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("MusicSchool.Finance.Api")
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<Invoice>, InvoiceRepository>();

builder.Services.AddHttpClient<ISchoolManagementClient, SchoolManagementClient>(
    client => client.BaseAddress = new Uri(builder.Configuration["SchoolManagementApiUrl"])
);

builder.Services.AddScoped<IInvoiceService, InvoiceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
