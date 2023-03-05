using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Api.SwaggerGen;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;
using MusicSchool.Finance.Infrastructure.DataAccess;
using MusicSchool.Finance.Infrastructure.External.SchoolManagement;
using MusicSchool.Finance.Infrastructure.Json;
using MusicSchool.Finance.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FinanceContext>(options =>
{
    options.UseMySQL(
        builder.Configuration.GetConnectionString("Default") ?? "",
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
builder.Services.AddSwaggerGen(SwaggerGenHelper.Setup);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddHttpClient<ISchoolManagementClient, SchoolManagementClient>(
    client => client.BaseAddress = new Uri(builder.Configuration["SchoolManagementApiUrl"] ?? "")
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
