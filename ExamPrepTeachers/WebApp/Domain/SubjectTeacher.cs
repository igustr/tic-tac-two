namespace ExamPrepTeachers.Domain;

public class SubjectTeacher
{
    public int Id { get; set; }
    
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    
    public int SubjectId { get; set; }
    public Subjects Subject { get; set; }
}