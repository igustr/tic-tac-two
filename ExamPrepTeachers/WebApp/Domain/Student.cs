using System.ComponentModel.DataAnnotations;

namespace ExamPrepTeachers.Domain;

public class Student
{
    public int Id { get; set; }

    
    [MaxLength(128)]
    public string Name { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}