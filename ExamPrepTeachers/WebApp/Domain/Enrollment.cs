namespace ExamPrepTeachers.Domain;

public class Enrollment
{
        public int Id { get; set; }
        
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        public Subjects Subjects { get; set; }

        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public string FinalGrade { get; set; } // Pass/Fail or Graded
        public ICollection<Grade> Grades { get; set; }
}