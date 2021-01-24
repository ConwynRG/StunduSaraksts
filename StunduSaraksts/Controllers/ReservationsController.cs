using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StunduSaraksts.ModelsDB;
using StunduSaraksts.Models;

namespace StunduSaraksts.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public ReservationsController(StunduSarakstsContext context)
        {
            _context = context;
        }


        // GET: Reservations
        public async Task<IActionResult> Index()
        {

            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //Trace.WriteLine(currentUser);
            //Trace.Write("Id: ");
            //Trace.WriteLine(User.Identity.Name);
            ViewData["user"] = currentUser;
            var stunduSarakstsContext = _context.Reservations.Include(r => r.OwnerNavigation).Include(r => r.RoomNavigation);
            return View(await stunduSarakstsContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.OwnerNavigation)
                .Include(r => r.RoomNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Room,Date,StartTime,EndTime,RequestComment")] ReservationForm reservationForm)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                Reservation reservation = new Reservation();
                reservation.Owner = currentUser.Id;
                reservation.Room = reservationForm.Room;
                reservation.StartTime = reservationForm.Date.Add(reservationForm.StartTime);
                reservation.EndTime = reservationForm.Date.Add(reservationForm.EndTime);
                reservation.RequestDate = DateTime.Now;
                reservation.RequestComment = reservationForm.RequestComment;
                reservation.Canceled = false;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Room"] = new SelectList(_context.Rooms, "Id", "Name", reservationForm.Room);
            return View();
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Owner"] = new SelectList(_context.AspNetUsers, "Id", "Id", reservation.Owner);
            ViewData["Room"] = new SelectList(_context.Rooms, "Id", "Name", reservation.Room);
            ViewData["Duration"] = reservation.EndTime.TimeOfDay - reservation.StartTime.TimeOfDay;
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Owner,Room,StartTime,EndTime,RequestDate,ReplyDate,RequestComment,ReplyComment,Accepted,Canceled")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["Owner"] = new SelectList(_context.AspNetUsers, "Id", "Id", reservation.Owner);
            ViewData["Room"] = new SelectList(_context.Rooms, "Id", "Name", reservation.Room);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.OwnerNavigation)
                .Include(r => r.RoomNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
