using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Infrastructure.Repositories;
using MusicSchool.SchoolManagement.Repositories;

SchoolManagementContextFactory contextFactory = new();

using SchoolManagementContext context = contextFactory.CreateDbContext(args);

IRepository<Student> studentRepo = new Repository<Student>(context);
IRepository<Course> courseRepo = new Repository<Course>(context);
IRepository<Enrollment> enrollmentRepo = new Repository<Enrollment>(context);

IStudentService studentService = new StudentService(studentRepo);
ICourseService courseService = new CourseService(courseRepo);
IEnrollmentService enrollmentService = new EnrollmentService(
    enrollmentRepo,
    studentRepo,
    courseRepo
);

Student student = await studentService.CreateAsync("Luiz Melodia");
Course course = await courseService.CreateAsync("Técnica Vocal");
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
