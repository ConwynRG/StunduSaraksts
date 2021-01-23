using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Reservation
    {
        public Reservation()
        {
            Consultations = new HashSet<Consultation>();
        }

        public int Id { get; set; }
        public string Owner { get; set; }
        public int Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string RequestComment { get; set; }
        public string ReplyComment { get; set; }
        public bool? Accepted { get; set; }
        public bool? Canceled { get; set; }

        public virtual AspNetUser OwnerNavigation { get; set; }
        public virtual Room RoomNavigation { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}
