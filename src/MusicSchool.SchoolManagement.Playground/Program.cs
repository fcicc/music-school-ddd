using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Factories;
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
IEnrollmentFactory enrollmentFactory = new EnrollmentFactory();

Student student = studentFactory.CreateStudent("Luiz Melodia");
Course course = courseFactory.CreateCourse("Técnica Vocal");

Enrollment enrollment = enrollmentFactory.CreateEnrollment(
    student,
    course,
    new DateOnly(2023, 1, 1),
    new DateOnly(2023, 12, 31),
    4,
    250
);

await studentRepo.AddAsync(student);
await courseRepo.AddAsync(course);
await enrollmentRepo.AddAsync(enrollment);

Student persistedStudent = await studentRepo.FindOneAsync(student.Id);
Course persistedCourse = await courseRepo.FindOneAsync(course.Id);
Enrollment persistedEnrollment = await enrollmentRepo.FindOneAsync(enrollment.Id);

Console.WriteLine("Information about persisted student:");
Console.WriteLine($"ID   = \"{persistedStudent.Id}\"");
Console.WriteLine($"Name = \"{persistedStudent.Name}\"");

Console.WriteLine("Information about persisted course:");
Console.WriteLine($"ID   = \"{persistedCourse.Id}\"");
Console.WriteLine($"Name = \"{persistedCourse.Name}\"");

Console.WriteLine("Information about persisted enrollment:");
Console.WriteLine($"ID                = \"{persistedEnrollment.Id}\"");
Console.WriteLine($"Student ID        = \"{persistedEnrollment.StudentId}\"");
Console.WriteLine($"Course ID         = \"{persistedEnrollment.CourseId}\"");
Console.WriteLine($"Start Date        = \"{persistedEnrollment.StartDate}\"");
Console.WriteLine($"End Date          = \"{persistedEnrollment.EndDate}\"");
Console.WriteLine($"Lessons per Month = \"{persistedEnrollment.LessonsPerMonth}\"");
Console.WriteLine($"Monthly Bill      = \"{persistedEnrollment.MonthlyBill}\"");
