using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamPrepTeachers.DAL;
using ExamPrepTeachers.Domain;

namespace ExamPrepTeachers.Pages_Enrollment
{
    public class DetailsModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public DetailsModel(ExamPrepTeachers.DAL.AppDbContext context)
        {
            _context = context;
        }

        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment is not null)
            {
                Enrollment = enrollment;

                return Page();
            }

            return NotFound();
        }
    }
}
