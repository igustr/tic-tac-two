using System.ComponentModel.DataAnnotations;

namespace ExamPrepTeachers.Domain;

public class Semester
{
    public int Id { get; set; }

    
    [MaxLength(128)]
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}