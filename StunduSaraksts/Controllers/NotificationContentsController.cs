using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StunduSaraksts.ModelsDB;

namespace StunduSaraksts.Controllers
{
    public class NotificationContentsController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public NotificationContentsController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: NotificationContents
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.NotificationContents.ToListAsync());
        }

        // GET: NotificationContents/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationContent = await _context.NotificationContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationContent == null)
            {
                return NotFound();
            }

            return View(notificationContent);
        }

        // GET: NotificationContents/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Text")] NotificationContent notificationContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationContent);
        }

        // GET: NotificationContents/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationContent = await _context.NotificationContents.FindAsync(id);
            if (notificationContent == null)
            {
                return NotFound();
            }
            return View(notificationContent);
        }

        // POST: NotificationContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] NotificationContent notificationContent)
        {
            if (id != notificationContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationContentExists(notificationContent.Id))
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
            return View(notificationContent);
        }

        // GET: NotificationContents/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationContent = await _context.NotificationContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationContent == null)
            {
                return NotFound();
            }

            return View(notificationContent);
        }

        // POST: NotificationContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notificationContent = await _context.NotificationContents.FindAsync(id);
            _context.NotificationContents.Remove(notificationContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationContentExists(int id)
        {
            return _context.NotificationContents.Any(e => e.Id == id);
        }
    }
}
