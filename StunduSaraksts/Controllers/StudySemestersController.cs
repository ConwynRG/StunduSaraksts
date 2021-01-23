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
    public class StudySemestersController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public StudySemestersController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: StudySemesters
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudySemesters.ToListAsync());
        }

        // GET: StudySemesters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySemester = await _context.StudySemesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studySemester == null)
            {
                return NotFound();
            }

            return View(studySemester);
        }

        // GET: StudySemesters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudySemesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate")] StudySemester studySemester)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studySemester);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studySemester);
        }

        // GET: StudySemesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySemester = await _context.StudySemesters.FindAsync(id);
            if (studySemester == null)
            {
                return NotFound();
            }
            return View(studySemester);
        }

        // POST: StudySemesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate")] StudySemester studySemester)
        {
            if (id != studySemester.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studySemester);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudySemesterExists(studySemester.Id))
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
            return View(studySemester);
        }

        // GET: StudySemesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySemester = await _context.StudySemesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studySemester == null)
            {
                return NotFound();
            }

            return View(studySemester);
        }

        // POST: StudySemesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studySemester = await _context.StudySemesters.FindAsync(id);
            _context.StudySemesters.Remove(studySemester);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudySemesterExists(int id)
        {
            return _context.StudySemesters.Any(e => e.Id == id);
        }
    }
}
