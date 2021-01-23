using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StunduSaraksts.ModelsDB;

namespace StunduSaraksts.Controllers
{
    public class TeacherSubjectsController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public TeacherSubjectsController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: TeacherSubjects
        public async Task<IActionResult> Index()
        {
            var stunduSarakstsContext = _context.TeacherSubjects.Include(t => t.StudySemesterNavigation).Include(t => t.SubjectNavigation).Include(t => t.TeacherNavigation);
            return View(await stunduSarakstsContext.ToListAsync());
        }

        // GET: TeacherSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects
                .Include(t => t.StudySemesterNavigation)
                .Include(t => t.SubjectNavigation)
                .Include(t => t.TeacherNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Create
        public IActionResult Create()
        {
            ViewData["StudySemester"] = new SelectList(_context.StudySemesters, "Id", "Name");
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name");
            ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account");
            return View();
        }

        // POST: TeacherSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Teacher,Subject,StudySemester")] TeacherSubject teacherSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudySemester"] = new SelectList(_context.StudySemesters, "Id", "Name", teacherSubject.StudySemester);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", teacherSubject.Subject);
            ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account", teacherSubject.Teacher);
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects.FindAsync(id);
            if (teacherSubject == null)
            {
                return NotFound();
            }
            ViewData["StudySemester"] = new SelectList(_context.StudySemesters, "Id", "Name", teacherSubject.StudySemester);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", teacherSubject.Subject);
            ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account", teacherSubject.Teacher);
            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Teacher,Subject,StudySemester")] TeacherSubject teacherSubject)
        {
            if (id != teacherSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherSubjectExists(teacherSubject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudySemester"] = new SelectList(_context.StudySemesters, "Id", "Name", teacherSubject.StudySemester);
            ViewData["Subject"] = new SelectList(_context.Subjects, "Id", "Name", teacherSubject.Subject);
            ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account", teacherSubject.Teacher);
            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherSubject = await _context.TeacherSubjects
                .Include(t => t.StudySemesterNavigation)
                .Include(t => t.SubjectNavigation)
                .Include(t => t.TeacherNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherSubject == null)
            {
                return NotFound();
            }

            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherSubject = await _context.TeacherSubjects.FindAsync(id);
            _context.TeacherSubjects.Remove(teacherSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherSubjectExists(int id)
        {
            return _context.TeacherSubjects.Any(e => e.Id == id);
        }
    }
}
