namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Student : IAggregateRoot
{
    public Guid Id { get; init; }

    public string Name { get; init; } = "";
}
