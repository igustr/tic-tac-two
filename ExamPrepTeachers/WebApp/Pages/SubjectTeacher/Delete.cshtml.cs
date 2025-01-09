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
    public class DeleteModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public DeleteModel(ExamPrepTeachers.DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubjectTeacher SubjectTeacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectteacher = await _context.SubjectTeacher.FirstOrDefaultAsync(m => m.Id == id);

            if (subjectteacher is not null)
            {
                SubjectTeacher = subjectteacher;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectteacher = await _context.SubjectTeacher.FindAsync(id);
            if (subjectteacher != null)
            {
                SubjectTeacher = subjectteacher;
                _context.SubjectTeacher.Remove(SubjectTeacher);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
