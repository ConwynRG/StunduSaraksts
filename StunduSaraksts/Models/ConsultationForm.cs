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
        [Remote("ConsultationExistingReservationCheck", "Validation",AdditionalFields ="Id,isOnline,Date,StartTime,EndTime")]
        public int? Room { get; set; }

        [Required(ErrorMessage = "Konsultācijas datums nevar būt tukšs.")]
        [Remote("ConsultationActualDate", "Validation", ErrorMessage = "Konsultāciju ir jāreģestrē vismaz 24 stundas pirms konsultācijas sākuma")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Konsultācijas sākuma laiks nevar būt tukšs.")]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Konsultācija beiguma laiks nevar būt tukšs.")]
        [Remote("ConsultationIntervalCheck","Validation",AdditionalFields ="StartTime",ErrorMessage ="Konsultācijas beigu laiks nevar būr agrāks par sākuma laiku")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Konsultāciju apraksts nevar būt tukšs.")]
        public string Comment { get; set; }
    }
}
