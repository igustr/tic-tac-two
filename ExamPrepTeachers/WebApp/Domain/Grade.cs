using System.ComponentModel.DataAnnotations;

namespace ExamPrepTeachers.Domain;

public class Grade
{
    public int Id { get; set; }
    
    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; }

    [MaxLength(128)]
    public string TaskName { get; set; }
    public double Score { get; set; }
}