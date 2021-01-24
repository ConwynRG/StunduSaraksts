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
        [Route("ConsultationAttendances/{id}/Create")]
        public IActionResult Create(int id)
        {
            ConsultationAttendance consultationAttendance = new ConsultationAttendance();
            consultationAttendance.Consultation = id;
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!currentUser.IsStudent()) ViewBag.ErrorMessage = "Pieteikties vai Atteikties no konsultācijas ir iespējams tikai skolēnam";

            return View(consultationAttendance);
        }

        // POST: ConsultationAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("ConsultationAttendances/{id}/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Consultation,Comment")] ConsultationAttendance consultationAttendance)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                if(!currentUser.IsStudent()) return RedirectToAction("Index", "Consultations");

                consultationAttendance.Attends = true;
                consultationAttendance.RegisterDate = DateTime.Now;
                consultationAttendance.Student = currentUser.GetStudent().Id; 
                _context.Add(consultationAttendance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Consultations");
            }
            return View(consultationAttendance);
        }

        // GET: ConsultationAttendances/Edit/5
        [Route("ConsultationAttendances/{cons_id}/Edit/{id}")]
        public async Task<IActionResult> Edit(int cons_id,int id)
        {

            
            var consultationAttendance = await _context.ConsultationAttendances.FindAsync(id);
            if (consultationAttendance == null || consultationAttendance.Consultation != cons_id)
            {
                return NotFound();
            }
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!currentUser.IsStudent()) ViewBag.ErrorMessage = "Pieteikties vai Atteikties no konsultācijas ir iespējams tikai skolēnam";

            return View(consultationAttendance);
        }

        // POST: ConsultationAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("ConsultationAttendances/{cons_id}/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Consultation,Comment")] ConsultationAttendance consultationAttendance)
        {
            if (id != consultationAttendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    if (!currentUser.IsStudent()) return RedirectToAction("Index", "Consultations");

                    var attendance = await _context.ConsultationAttendances.FindAsync(id);
                    attendance.Attends = !attendance.Attends;
                    attendance.Comment = consultationAttendance.Comment;
                    attendance.RegisterDate = DateTime.Now;
                    _context.Update(attendance);
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
                return RedirectToAction("Index","Consultations");
            }
            
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
