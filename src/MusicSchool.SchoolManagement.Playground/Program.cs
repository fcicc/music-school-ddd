using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Factories;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;
using MusicSchool.SchoolManagement.Repositories;

SchoolManagementContextFactory contextFactory = new();

using SchoolManagementContext context = contextFactory.CreateDbContext(args);

IRepository<Student> studentRepo = new Repository<Student>(context);

IStudentFactory studentFactory = new StudentFactory();

Student student = studentFactory.CreateStudent("Luiz Melodia");

await studentRepo.AddAsync(student);

Student persistedStudent = await studentRepo.FindOneAsync(student.Id);

Console.WriteLine("Information about persisted student:");
Console.WriteLine($"ID   = \"{persistedStudent.Id}\"");
Console.WriteLine($"Name = \"{persistedStudent.Name}\"");
