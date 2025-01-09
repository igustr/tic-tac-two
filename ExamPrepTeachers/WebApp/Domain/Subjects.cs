using System.ComponentModel.DataAnnotations;

namespace ExamPrepTeachers.Domain;

public class Subjects
{
    public int Id { get; set; }

    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    [MaxLength(10240)]
    public string Description { get; set; }
    public int ECTS { get; set; }
    public ICollection<SubjectTeacher> SubjectTeachers { get; set; }
}