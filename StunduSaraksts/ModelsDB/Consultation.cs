using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Consultation
    {
        public int Id { get; set; }
        public int Teacher { get; set; }
        public int? RoomReservation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Comment { get; set; }
        public bool? Canceled { get; set; }

        public virtual Reservation RoomReservationNavigation { get; set; }
        public virtual Teacher TeacherNavigation { get; set; }
    }
}
