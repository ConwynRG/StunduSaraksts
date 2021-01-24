using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StunduSaraksts.Models;
using StunduSaraksts.ModelsDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace StunduSaraksts.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly StunduSarakstsContext _context;

        public ConsultationsController(StunduSarakstsContext context)
        {
            _context = context;
        }

        // GET: Consultations
        public async Task<IActionResult> Index()
        {
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewData["user"] = currentUser;
            var stunduSarakstsContext = _context.Consultations.Include(c => c.RoomReservationNavigation)
                                                                    .ThenInclude(rr => rr.RoomNavigation)
                                                              .Include(c => c.TeacherNavigation)
                                                                    .ThenInclude(t => t.AccountNavigation);
            return View(await stunduSarakstsContext.ToListAsync());
        }

        // GET: Consultations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .Include(c => c.RoomReservationNavigation)
                .Include(c => c.TeacherNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        [Authorize]
        public IActionResult Create()
        {
            //var teachers = _context.Teachers.Include(t => t.AccountNavigation);
            //ViewData["Teachers"] = new SelectList(teachers, "Id", "FullName");
            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name");
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser.IsStudent()) ViewBag.ErrorMessage = "Studentam nav tiesību veidot jaunu konsultāciju. Konsultāciju izveidošana ir iespējama tikai skolotājam vai sistēmas administratoram.";

            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("isOnline,Room,Date,StartTime,EndTime,Comment")] ConsultationForm consultationRequest)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                if (currentUser.IsStudent()) return RedirectToAction(nameof(Index));
                Consultation consultation = new Consultation();
                consultation.RegisterDate = DateTime.Now;
                consultation.Teacher = currentUser.GetTeacher().Id;
                if(consultationRequest.isOnline == false && consultationRequest.Room.HasValue)
                {
                    Reservation reservation = new Reservation();
                    reservation.Owner = currentUser.Id;
                    reservation.Room = consultationRequest.Room.Value;
                    reservation.RequestDate = DateTime.Now;
                    reservation.StartTime = consultationRequest.Date.Add(consultationRequest.StartTime);
                    reservation.EndTime = consultationRequest.Date.Add(consultationRequest.EndTime);
                    reservation.RequestComment = "Konsultācija: " + consultationRequest.Comment;
                    reservation.Canceled = false;
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    consultation.RoomReservation = reservation.Id;
                }
                else
                {
                    consultation.RoomReservation = null; 
                }
                consultation.StartTime = consultationRequest.Date.Add(consultationRequest.StartTime);
                consultation.EndTime = consultationRequest.Date.Add(consultationRequest.EndTime);
                consultation.Comment = consultationRequest.Comment;
                consultation.Canceled = false;
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //var teachers = _context.Teachers.Include(t => t.AccountNavigation);
            //ViewData["Teachers"] = new SelectList(teachers, "Id", "FullName", consultationRequest.Teacher);
            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name", consultationRequest.Room);
            return View();
        }

        // GET: Consultations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations.FindAsync(id);

            if (consultation == null)
            {
                return NotFound();
            }

            ConsultationForm consultationForm = new ConsultationForm();
            consultationForm.Id = consultation.Id;

            if (consultation.RoomReservation != null)
            {
                consultationForm.isOnline = false;
                consultationForm.Room = (await _context.Reservations.Where(res => res.Id == consultation.RoomReservation).FirstOrDefaultAsync()).Room;
            }
            else
            {
                consultationForm.isOnline = true;
                consultationForm.Room = null;
            }
            consultationForm.Date = consultation.StartTime.Date;
            consultationForm.StartTime = consultation.StartTime.TimeOfDay;
            consultationForm.EndTime = consultation.EndTime.TimeOfDay;
            consultationForm.Comment = consultation.Comment;

            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name", consultationForm.Room);
            // ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account", consultation.Teacher);
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser.IsStudent()) ViewBag.ErrorMessage = "Studentam nav tiesību rediģēt konsultāciju. Konsultāciju rediģēšana ir iespējama tikai skolotājam vai sistēmas administratoram.";
            if(!currentUser.IsAdmin() && currentUser.IsTeacher() && currentUser.GetTeacher().Id != consultation.Teacher) ViewBag.ErrorMessage = "Skolotājam ir iespējams rediģēt tikai savas izveidotas konsultācijas.";
            return View(consultationForm);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,isOnline,Room,Date,StartTime,EndTime,Comment")] ConsultationForm consultationForm)
        {
            if (id != consultationForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var consultation = await _context.Consultations.FindAsync(id);

                    var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    if (currentUser.IsStudent() || (!currentUser.IsAdmin() && currentUser.IsTeacher() && currentUser.GetTeacher().Id != consultation.Teacher)) return RedirectToAction(nameof(Index));
                    
                    DateTime startCon = consultationForm.Date.Add(consultationForm.StartTime);
                    DateTime endCon = consultationForm.Date.Add(consultationForm.EndTime);
                    var reservation = (await _context.Reservations.Where(res => res.Id == consultation.RoomReservation).FirstOrDefaultAsync());
                    bool canceled = false; 

                    if(consultation.RoomReservation != null && (consultationForm.isOnline || 
                        reservation.Room != consultationForm.Room || consultation.StartTime != startCon || consultation.EndTime != endCon))
                    {
                        //Cancel previous reservation for consultation
                        reservation.Canceled = true;
                        _context.Update(reservation);
                        consultation.RoomReservation = null;
                        _context.Update(consultation);
                        await _context.SaveChangesAsync();
                        canceled = true;
                    }

                    if ((!consultationForm.isOnline && canceled) || (consultation.RoomReservation == null && !consultationForm.isOnline))
                    {
                        Reservation newReservation = new Reservation();
                        newReservation.Owner = currentUser.Id;
                        newReservation.Room = consultationForm.Room.Value;
                        newReservation.RequestDate = DateTime.Now;
                        newReservation.StartTime = startCon;
                        newReservation.EndTime = endCon;
                        newReservation.RequestComment = "Konsultācija: " + consultationForm.Comment;
                        newReservation.Canceled = false;
                        _context.Add(newReservation);
                        await _context.SaveChangesAsync();
                        consultation.RoomReservation = newReservation.Id;
                    }
                    consultation.StartTime = startCon;
                    consultation.EndTime = endCon;
                    consultation.Comment = consultationForm.Comment;
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                    
                    NotificationContent notificationContent = new NotificationContent();
                    notificationContent.Text = "Konsultācija, kura tika ieplānota " + consultation.StartTime.Day + "/" + consultation.StartTime.Month + "/" + consultation.StartTime + " "
                        + consultation.StartTime.TimeOfDay.ToString() + ", tika rediģēta \n";

                    if (consultationForm.isOnline) notificationContent.Text += " Notiek tiešsaistē \n";
                    else
                    {
                        var roomName = _context.Rooms.Find(consultationForm.Room.Value).Name;
                        notificationContent.Text += "Kabinets: " + roomName + "\n";
                    }
                    notificationContent.Text += "Laiks un Datums: " + consultation.StartTime.Day +"/"+consultation.StartTime.Month+"/"+consultation.StartTime.Year+" "
                        +consultation.StartTime.TimeOfDay+"\n"+"Konsultācijas apraksts: " + consultation.Comment;

                    _context.Add(notificationContent);
                    await _context.SaveChangesAsync();

                    var attendedStudents = _context.ConsultationAttendances.Include(ca => ca.StudentNavigation).Where(ca => ca.Consultation == consultation.Id && ca.Attends == true).ToList();
                    foreach (var student in attendedStudents)
                    {
                        Notification notification = new Notification();
                        notification.Sender = currentUser.Id;
                        notification.Recipient = student.StudentNavigation.Account;
                        notification.Content = notificationContent.Id;
                        notification.SentTime = DateTime.Now;
                        _context.Add(notification);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultationForm.Id))
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
            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name", consultationForm.Room);
            //ViewData["Teacher"] = new SelectList(_context.Teachers, "Id", "Account", consultation.Teacher);
            return View(consultationForm);
        }

        // GET: Consultations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .Include(c => c.RoomReservationNavigation)
                .Include(c => c.TeacherNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            ViewBag.ConsultationId = consultation.Id;
            var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser.IsStudent()) ViewBag.ErrorMessage = "Studentam nav tiesību atcelt konsultāciju. Konsultāciju atcelšana ir iespējama tikai skolotājam vai sistēmas administratoram.";
            if (!currentUser.IsAdmin() && currentUser.IsTeacher() && currentUser.GetTeacher().Id != consultation.Teacher) ViewBag.ErrorMessage = "Skolotājam ir iespējams atcelt tikai savas izveidotas konsultācijas.";
            return View();
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,[Bind("Text")] NotificationContent notificationContent)
        {
            if (ModelState.IsValid)
            {
                var consultation = await _context.Consultations.FindAsync(id);
                var currentUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                if (currentUser.IsTeacher() && currentUser.GetTeacher().Id == consultation.Teacher)
                {
                    if (consultation.RoomReservation != null)
                    {
                        var reservation = await _context.Reservations.Where(res => res.Id == consultation.RoomReservation).FirstOrDefaultAsync();
                        reservation.Canceled = true;
                        _context.Update(reservation);
                        await _context.SaveChangesAsync();
                    }
                    consultation.Canceled = true;
                    _context.Update(consultation);
                    notificationContent.Text = "Konsultācija, kura tika ieplānota " + consultation.StartTime.Day + "/" + consultation.StartTime.Month + "/" + consultation.StartTime + " "
                        + consultation.StartTime.TimeOfDay.ToString() + ", tika atcelta \n Skolotāja komentārs: " + notificationContent.Text;
                    _context.Add(notificationContent);
                    await _context.SaveChangesAsync();


                    var attendedStudents = _context.ConsultationAttendances.Include(ca => ca.StudentNavigation).Where(ca => ca.Consultation == consultation.Id && ca.Attends == true).ToList();
                    foreach (var student in attendedStudents)
                    {
                        Notification notification = new Notification();
                        notification.Sender = currentUser.Id;
                        notification.Recipient = student.StudentNavigation.Account;
                        notification.Content = notificationContent.Id;
                        notification.SentTime = DateTime.Now;
                        _context.Add(notification);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultations.Any(e => e.Id == id);
        }
    }
}
