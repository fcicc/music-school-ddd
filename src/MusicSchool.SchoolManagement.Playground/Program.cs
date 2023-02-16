using Microsoft.Extensions.Configuration;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

using SchoolManagementContext context = new(config);

StudentRepository studentRepo = new(context);

Student student = new()
{
    Id = Guid.NewGuid(),
    Name = "Luiz Melodia",
};

await studentRepo.CreateAsync(student);

Student persistedStudent = await studentRepo.FindOneAsync(student.Id);

Console.WriteLine("Information about persisted student:");
Console.WriteLine($"ID   = \"{persistedStudent.Id}\"");
Console.WriteLine($"Name = \"{persistedStudent.Name}\"");
