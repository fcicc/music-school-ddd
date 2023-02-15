// See https://aka.ms/new-console-template for more information
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;

StudentRepository studentRepo = new();

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
