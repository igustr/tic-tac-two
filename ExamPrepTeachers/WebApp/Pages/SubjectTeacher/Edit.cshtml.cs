using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamPrepTeachers.DAL;
using ExamPrepTeachers.Domain;

namespace ExamPrepTeachers.Pages_SubjectTeacher
{
    public class EditModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public EditModel(ExamPrepTeachers.DAL.AppDbContext context)
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

            var subjectteacher =  await _context.SubjectTeacher.FirstOrDefaultAsync(m => m.Id == id);
            if (subjectteacher == null)
            {
                return NotFound();
            }
            SubjectTeacher = subjectteacher;
           ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id");
           ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubjectTeacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectTeacherExists(SubjectTeacher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SubjectTeacherExists(int id)
        {
            return _context.SubjectTeacher.Any(e => e.Id == id);
        }
    }
}
