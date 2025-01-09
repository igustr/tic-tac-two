using ExamPrepTeachers.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepTeachers.DAL;

public class AppDbContext: DbContext
{
    public DbSet<Teacher> Teachers { get; set; } = default!;
    public DbSet<Grade> Grade { get; set; } = default!;
    public DbSet<SubjectTeacher> SubjectTeacher { get; set; } = default!;
    public DbSet<Subjects> Subjects { get; set; } = default!;
    public DbSet<Semester> Semester { get; set; } = default!;
    public DbSet<Student> Student { get; set; } = default!;
    public DbSet<Enrollment> Enrollment { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //dotnet ef migrations add InitialCreate --project WebApp --startup-project WebApp
    }
}