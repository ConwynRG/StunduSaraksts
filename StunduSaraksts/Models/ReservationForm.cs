using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StunduSaraksts.Models
{
    public class ReservationForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ir jāizvēlās kabinetu priekš rezervācijas")]
        [Remote("ExistingRerservationCheck","Validation", AdditionalFields = "Date,StartTime,EndTime")]
        public int Room { get; set; }
        
        [Required(ErrorMessage ="Kabineta rezervācijas datums nevar būt tukšs")]
        [Remote("ActualDateCheck","Validation",ErrorMessage = "Kabineta rezervācija ir jāreģestrē vismaz 24 stundas pirms rezervācijas sākuma")]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage ="Kabineta rezervācijas sākuma laiks nevar būt tukšs")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage ="Kabineta rezervācijas beigu laiks nevār būt tukšs")]
        [Remote("IntervalCheck","Validation",AdditionalFields ="StartTime",ErrorMessage = "Kabineta rezervācijas beigu laiks nevar būr agrāks par sākuma laiku")]
        public TimeSpan EndTime { get; set; }
        [Required(ErrorMessage ="Kabineta rezervācijas apraksts nevar būt tukšs")]
        public string RequestComment { get; set; }
    }
}

