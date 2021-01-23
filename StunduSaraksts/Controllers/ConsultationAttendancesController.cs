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
    public class ConsultationAttendancesController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public ConsultationAttendancesController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: ConsultationAttendances
        public async Task<IActionResult> Index()
        {
            var stunduSarakstsContext = _context.ConsultationAttendances.Include(c => c.ConsultationNavigation).Include(c => c.StudentNavigation);
            return View(await stunduSarakstsContext.ToListAsync());
        }

        // GET: ConsultationAttendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultationAttendance = await _context.ConsultationAttendances
                .Include(c => c.ConsultationNavigation)
                .Include(c => c.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultationAttendance == null)
            {
                return NotFound();
            }

            return View(consultationAttendance);
        }

        // GET: ConsultationAttendances/Create
        public IActionResult Create()
        {
            ViewData["Consultation"] = new SelectList(_context.Consultations, "Id", "Comment");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Account");
            return View();
        }

        // POST: ConsultationAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,Consultation,Comment,RegisterDate,Attends")] ConsultationAttendance consultationAttendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultationAttendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Consultation"] = new SelectList(_context.Consultations, "Id", "Comment", consultationAttendance.Consultation);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Account", consultationAttendance.Student);
            return View(consultationAttendance);
        }

        // GET: ConsultationAttendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultationAttendance = await _context.ConsultationAttendances.FindAsync(id);
            if (consultationAttendance == null)
            {
                return NotFound();
            }
            ViewData["Consultation"] = new SelectList(_context.Consultations, "Id", "Comment", consultationAttendance.Consultation);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Account", consultationAttendance.Student);
            return View(consultationAttendance);
        }

        // POST: ConsultationAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Student,Consultation,Comment,RegisterDate,Attends")] ConsultationAttendance consultationAttendance)
        {
            if (id != consultationAttendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultationAttendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationAttendanceExists(consultationAttendance.Id))
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
            ViewData["Consultation"] = new SelectList(_context.Consultations, "Id", "Comment", consultationAttendance.Consultation);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Account", consultationAttendance.Student);
            return View(consultationAttendance);
        }

        // GET: ConsultationAttendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultationAttendance = await _context.ConsultationAttendances
                .Include(c => c.ConsultationNavigation)
                .Include(c => c.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultationAttendance == null)
            {
                return NotFound();
            }

            return View(consultationAttendance);
        }

        // POST: ConsultationAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultationAttendance = await _context.ConsultationAttendances.FindAsync(id);
            _context.ConsultationAttendances.Remove(consultationAttendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationAttendanceExists(int id)
        {
            return _context.ConsultationAttendances.Any(e => e.Id == id);
        }
    }
}
