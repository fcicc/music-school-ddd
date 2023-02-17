namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Student : IAggregateRoot
{
    internal Student() { }

    public Guid Id { get; init; }

    public string Name { get; init; } = "";
}
