using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Factories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;
using MusicSchool.SchoolManagement.Repositories;

SchoolManagementContextFactory contextFactory = new();

using SchoolManagementContext context = contextFactory.CreateDbContext(args);

IRepository<Student> studentRepo = new Repository<Student>(context);
IRepository<Course> courseRepo = new Repository<Course>(context);
IRepository<Enrollment> enrollmentRepo = new Repository<Enrollment>(context);

IStudentFactory studentFactory = new StudentFactory();
ICourseFactory courseFactory = new CourseFactory();

IEnrollmentService enrollmentService = new EnrollmentService(enrollmentRepo);

Student student = studentFactory.CreateStudent("Luiz Melodia");
Course course = courseFactory.CreateCourse("Técnica Vocal");

await studentRepo.AddAsync(student);
await courseRepo.AddAsync(course);

Enrollment enrollment = await enrollmentService.EnrollAsync(
    student.Id,
    course.Id,
    new DateOnly(2023, 1, 1),
    new DateOnly(2023, 12, 31),
    4,
    250
);

Console.WriteLine("Information about new student:");
Console.WriteLine($"ID   = {student.Id}");
Console.WriteLine($"Name = {student.Name}");
Console.WriteLine();
Console.WriteLine("Information about new course:");
Console.WriteLine($"ID   = {course.Id}");
Console.WriteLine($"Name = {course.Name}");
Console.WriteLine();
Console.WriteLine("Information about new enrollment:");
Console.WriteLine($"ID                = {enrollment.Id}");
Console.WriteLine($"Student ID        = {enrollment.StudentId}");
Console.WriteLine($"Course ID         = {enrollment.CourseId}");
Console.WriteLine($"Start Date        = {enrollment.StartDate}");
Console.WriteLine($"End Date          = {enrollment.EndDate}");
Console.WriteLine($"Lessons per Month = {enrollment.LessonsPerMonth}");
Console.WriteLine($"Monthly Bill      = {enrollment.MonthlyBill}");
