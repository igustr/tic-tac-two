using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamPrepTeachers.DAL;
using ExamPrepTeachers.Domain;

namespace ExamPrepTeachers.Pages_SubjectTeacher
{
    public class IndexModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public IndexModel(ExamPrepTeachers.DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<SubjectTeacher> SubjectTeacher { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SubjectTeacher = await _context.SubjectTeacher
                .Include(s => s.Subject)
                .Include(s => s.Teacher).ToListAsync();
        }
    }
}
