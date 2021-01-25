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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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
            ViewBag.reservation = reservation;
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!currentUser.IsAdmin()) ViewBag.ErrorMessage = "Kabineta rezervāciju ir iespējams apsiprināt vai noraidīt tikai sistēmas administratoram.";
            ReservationAdminForm reservationAdminForm = new ReservationAdminForm();
            reservationAdminForm.Id = reservation.Id;
            

            return View(reservationAdminForm);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReplyComment,Accepted")] ReservationAdminForm reservationAdminForm)
        {
            if (id != reservationAdminForm.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    if (!currentUser.IsAdmin()) return RedirectToAction(nameof(Index));

                    var reservation = await _context.Reservations
                       .Include(r => r.OwnerNavigation)
                       .Include(r => r.RoomNavigation)
                       .FirstOrDefaultAsync(m => m.Id == id);
                    reservation.ReplyDate = DateTime.Now;
                    reservation.ReplyComment = reservationAdminForm.ReplyComment;
                    reservation.Accepted = reservationAdminForm.Accepted;
                    _context.Update(reservation);

                    await _context.SaveChangesAsync();

                    NotificationContent contentForReservationOwner = new NotificationContent();
                    if (reservationAdminForm.Accepted)
                    {
                        contentForReservationOwner.Text = "Kabineta rezervācija priekš " + reservation.RoomNavigation.Name +
                            " kabineta tika apstiprināta uz laika intervālu " + reservation.StartTime.Day + "." + reservation.StartTime.Month + "." + reservation.StartTime.Year +
                            " no " + reservation.StartTime.TimeOfDay + " līdz " + reservation.EndTime.TimeOfDay;
                    }
                    else
                    {
                        contentForReservationOwner.Text = "Kabineta rezervācija priekš " + reservation.RoomNavigation.Name +
                            " kabineta tika noraidīta uz laika intervālu " + reservation.StartTime.Day + "." + reservation.StartTime.Month + "." + reservation.StartTime.Year +
                            " no " + reservation.StartTime.TimeOfDay + " līdz " + reservation.EndTime.TimeOfDay;
                    }
                    contentForReservationOwner.Text += " Administratora komentārs: " + reservation.ReplyComment;
                    _context.Update(contentForReservationOwner);
                    await _context.SaveChangesAsync();

                    Notification notification = new Notification();
                    notification.Sender = currentUser.Id;
                    notification.Recipient = reservation.OwnerNavigation.Id;
                    notification.Content = contentForReservationOwner.Id;
                    notification.SentTime = DateTime.Now;
                    _context.Add(notification);
                    await _context.SaveChangesAsync();

                    var consultation = _context.Consultations.Where(c => c.RoomReservation == reservation.Id).FirstOrDefault();
                    if(consultation != null)
                    {
                        NotificationContent contentForStudents = new NotificationContent();
                        if (reservationAdminForm.Accepted)
                        {
                            contentForStudents.Text = "Kabineta rezervācija priekš " + reservation.RoomNavigation.Name +
                                " kabineta, kurā tiks rīkota konsultācija, tika apstiprināta uz laika intervālu " + reservation.StartTime.Day + "." + reservation.StartTime.Month + "." + reservation.StartTime.Year +
                                " no " + reservation.StartTime.TimeOfDay + " līdz " + reservation.EndTime.TimeOfDay;
                        }
                        else
                        {
                            contentForStudents.Text = "Kabineta rezervācija priekš " + reservation.RoomNavigation.Name +
                                " kabineta, kurā tiks rīkota konsultācija, tika noraidīta uz laika intervālu " + reservation.StartTime.Day + "." + reservation.StartTime.Month + "." + reservation.StartTime.Year +
                                " no " + reservation.StartTime.TimeOfDay + " līdz " + reservation.EndTime.TimeOfDay;
                        }
                        _context.Update(contentForStudents);
                        await _context.SaveChangesAsync();

                        var attendedStudents = _context.ConsultationAttendances.Include(ca => ca.StudentNavigation).Where(ca => ca.Consultation == consultation.Id && ca.Attends == true).ToList();
                        foreach (var student in attendedStudents)
                        {
                            Notification notificationForStudent = new Notification();
                            notificationForStudent.Sender = currentUser.Id;
                            notificationForStudent.Recipient = student.StudentNavigation.Account;
                            notificationForStudent.Content = contentForStudents.Id;
                            notificationForStudent.SentTime = DateTime.Now;
                            _context.Add(notificationForStudent);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservationAdminForm.Id))
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
            
            return View(reservationAdminForm);
        }

        // GET: Reservations/Delete/5
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = _context.Reservations.Include(r => r.OwnerNavigation).Where(r => r.Id == id).FirstOrDefault();
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser.Id == reservation.Owner)
            {
                reservation.Canceled = true;
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
