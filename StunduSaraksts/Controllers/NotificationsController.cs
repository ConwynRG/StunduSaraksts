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
    public class NotificationsController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public NotificationsController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var stunduSarakstsContext = _context.Notifications.Include(n => n.ContentNavigation).Include(n => n.RecipientNavigation).Include(n => n.SenderNavigation);
            return View(await stunduSarakstsContext.ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.ContentNavigation)
                .Include(n => n.RecipientNavigation)
                .Include(n => n.SenderNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            ViewData["Content"] = new SelectList(_context.NotificationContents, "Id", "Text");
            ViewData["Recipient"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["Sender"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sender,Recipient,Content,SentTime,ReceivedTime")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Content"] = new SelectList(_context.NotificationContents, "Id", "Text", notification.Content);
            ViewData["Recipient"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Recipient);
            ViewData["Sender"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Sender);
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            ViewData["Content"] = new SelectList(_context.NotificationContents, "Id", "Text", notification.Content);
            ViewData["Recipient"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Recipient);
            ViewData["Sender"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Sender);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sender,Recipient,Content,SentTime,ReceivedTime")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
            ViewData["Content"] = new SelectList(_context.NotificationContents, "Id", "Text", notification.Content);
            ViewData["Recipient"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Recipient);
            ViewData["Sender"] = new SelectList(_context.AspNetUsers, "Id", "Id", notification.Sender);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.ContentNavigation)
                .Include(n => n.RecipientNavigation)
                .Include(n => n.SenderNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
