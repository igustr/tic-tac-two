using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamPrepTeachers.DAL;
using ExamPrepTeachers.Domain;

namespace ExamPrepTeachers.Pages_Subjects
{
    public class DeleteModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public DeleteModel(ExamPrepTeachers.DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subjects Subjects { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjects = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == id);

            if (subjects is not null)
            {
                Subjects = subjects;

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

            var subjects = await _context.Subjects.FindAsync(id);
            if (subjects != null)
            {
                Subjects = subjects;
                _context.Subjects.Remove(Subjects);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
