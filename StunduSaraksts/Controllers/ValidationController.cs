using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StunduSaraksts.ModelsDB;

namespace StunduSaraksts.Controllers
{
    public class ValidationController : Controller
    {
        private readonly StunduSarakstsContext _context;
        
        public ValidationController(StunduSarakstsContext context)
        {
            _context = context;
        } 
        
        public JsonResult ActualDateCheck(DateTime date)
        {
            if(DateTime.Compare(date,DateTime.Now) >= 0) return Json(true);
            else return Json(false);
        }

        public JsonResult IntervalCheck(TimeSpan startTime, TimeSpan endTime)
        {
            if (TimeSpan.Compare(startTime, endTime) < 0) return Json(true);
            else return Json(false);
        }

        public JsonResult ConsultationExistingReservationCheck(int id,int? room, bool isOnline, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            
            if (isOnline) return Json(true);
            else
            {
                if (room != null) {
                    if (date.Year == 1) date = DateTime.Now;
                    DateTime startCon = date.Add(startTime);
                    DateTime endCon = date.Add(endTime);
                    
                    Reservation reservation;
                    if (id == 0)
                    {
                        reservation = _context.Reservations.Where(r => r.Room == room &&
                        r.Accepted == true && r.Canceled == false && (
                        (DateTime.Compare(startCon, r.StartTime) > 0 && DateTime.Compare(startCon, r.EndTime) < 0) ||
                        (DateTime.Compare(endCon, r.StartTime) > 0 && DateTime.Compare(endCon, r.EndTime) < 0) ||
                        (DateTime.Compare(startCon, r.StartTime) < 0 && DateTime.Compare(endCon, r.EndTime) > 0)
                        )).FirstOrDefault();
                    }
                    else
                    {
                        var consultation = _context.Consultations.Find(id);
                        reservation = _context.Reservations.Where(r => r.Id != consultation.RoomReservation && r.Room == room && 
                        r.Accepted == true && r.Canceled == false && (
                        (DateTime.Compare(startCon, r.StartTime) > 0 && DateTime.Compare(startCon, r.EndTime) < 0) ||
                        (DateTime.Compare(endCon, r.StartTime) > 0 && DateTime.Compare(endCon, r.EndTime) < 0) ||
                        (DateTime.Compare(startCon, r.StartTime) < 0 && DateTime.Compare(endCon, r.EndTime) > 0)
                        )).FirstOrDefault();
                    }
                    if (reservation != null){
                        var roomObj = _context.Rooms.Find(room);
                        string message = "Konsultāciju nevar rīkot " + roomObj.Name + " kabinetā " + reservation.StartTime.Day 
                            + "." + reservation.StartTime.Month + "."+reservation.StartTime.Year
                            + " no " + reservation.StartTime.TimeOfDay.ToString() + " līdz " + reservation.EndTime.TimeOfDay.ToString();
                        return Json(message);
                    }
                    else
                    {
                        return Json(true);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
                    
        }

        public JsonResult ExistingRerservationCheck(int room, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (date.Year == 1) date = DateTime.Now;
            DateTime startCon = date.Add(startTime);
            DateTime endCon = date.Add(endTime);
            Reservation reservation = _context.Reservations.Where(r => r.Room == room &&
                r.Accepted == true && r.Canceled == false && (
                (DateTime.Compare(startCon, r.StartTime) > 0 && DateTime.Compare(startCon, r.EndTime) < 0) ||
                (DateTime.Compare(endCon, r.StartTime) > 0 && DateTime.Compare(endCon, r.EndTime) < 0) ||
                (DateTime.Compare(startCon, r.StartTime) < 0 && DateTime.Compare(endCon, r.EndTime) > 0)
                )).FirstOrDefault();
            if (reservation != null)
            {
                var roomObj = _context.Rooms.Find(room);
                string message = "Kabineta rezervāciju nevar veikt priekš " + roomObj.Name + " kabineta, jo tas tika rezervēts " + reservation.StartTime.Day
                    + "." + reservation.StartTime.Month + "." + reservation.StartTime.Year
                    + " no " + reservation.StartTime.TimeOfDay.ToString() + " līdz " + reservation.EndTime.TimeOfDay.ToString();
                return Json(message);
            }
            else
            {
                return Json(true);
            }
        }
    }
}
