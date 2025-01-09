using System.ComponentModel.DataAnnotations;

namespace ExamPrepTeachers.Domain;

public class Teacher
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public ICollection<SubjectTeacher> SubjectTeacher { get; set; }
}