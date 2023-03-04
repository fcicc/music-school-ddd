using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;
using MusicSchool.Finance.Domain.ValueObjects;
using MusicSchool.Finance.Infrastructure.DataAccess;
using MusicSchool.Finance.Infrastructure.External.SchoolManagement;
using MusicSchool.Finance.Infrastructure.Json;
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

builder.Services.AddHttpClient<ISchoolManagementClient, SchoolManagementClient>(
    client => client.BaseAddress = new Uri(builder.Configuration["SchoolManagementApiUrl"])
);

builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoicePaymentService, InvoicePaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
