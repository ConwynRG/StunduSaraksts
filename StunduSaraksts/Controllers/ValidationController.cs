﻿using Microsoft.AspNetCore.Mvc;
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
        
        public JsonResult ConsultationActualDate(DateTime date)
        {
            if(DateTime.Compare(date,DateTime.Now.AddDays(1)) >= 0) return Json(true);
            else return Json(false);
        }

        public JsonResult ConsultationIntervalCheck(TimeSpan startTime, TimeSpan endTime)
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
                    DateTime startCon = date.Add(startTime);
                    DateTime endCon = date.Add(endTime);
                    
                    Reservation reservation;
                    if (id == 0)
                    {
                        reservation = _context.Reservations.Where(r => r.Room == room && 
                        r.Accepted == true && r.Canceled == false && (
                        (DateTime.Compare(startCon, r.StartTime) > 0 && DateTime.Compare(startCon, r.EndTime) < 0) ||
                        (DateTime.Compare(endCon, r.StartTime) > 0 && DateTime.Compare(endCon, r.EndTime) < 0))
                        ).FirstOrDefault();
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
                        string message = "Consultation can't be held in room " + roomObj.Name + " on " + reservation.StartTime.Day 
                            + "/" + reservation.StartTime.Month + "/"+reservation.StartTime.Year
                            + " from " + reservation.StartTime.TimeOfDay.ToString() + " to " + reservation.EndTime.TimeOfDay.ToString();
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
    }
}
