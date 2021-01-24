using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StunduSaraksts.ModelsDB;

namespace StunduSaraksts.Models
{
    public class ConsultationForm
    {
        public int Id { get; set; }
        //public int Teacher { get; set; }
        [Required]
        public bool isOnline { get; set; }
        [Remote("ConsultationExistingReservationCheck", "Validation",AdditionalFields ="Id,isOnline,Date,StartTime,EndTime",ErrorMessage ="Reservation exists of this room exists in given time. Change room or time interval.")]
        public int? Room { get; set; }

        [Required(ErrorMessage = "Consultation date can't be empty.")]
        [Remote("ConsultationActualDate", "Validation", ErrorMessage = "Consultation should start at least day before actual date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Consultation start time can't be empty.")]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Consultation end time can't be empty.")]
        [Remote("ConsultationIntervalCheck","Validation",AdditionalFields ="StartTime",ErrorMessage ="Consultation end time can't be earlier than consultation start time.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Consultation description can't be empty.")]
        public string Comment { get; set; }
    }
}
