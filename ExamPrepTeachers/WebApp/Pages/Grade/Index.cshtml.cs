using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExamPrepTeachers.DAL;
using ExamPrepTeachers.Domain;

namespace ExamPrepTeachers.Pages_Grade
{
    public class IndexModel : PageModel
    {
        private readonly ExamPrepTeachers.DAL.AppDbContext _context;

        public IndexModel(ExamPrepTeachers.DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Grade> Grade { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Grade = await _context.Grade
                .Include(g => g.Enrollment).ToListAsync();
        }
    }
}
