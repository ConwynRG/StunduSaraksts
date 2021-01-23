using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Room
    {
        public Room()
        {
            Changes = new HashSet<Change>();
            Lessons = new HashSet<Lesson>();
            Reservations = new HashSet<Reservation>();
            RoomSubjectSpecializations = new HashSet<RoomSubjectSpecialization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public int StudySemester { get; set; }

        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<RoomSubjectSpecialization> RoomSubjectSpecializations { get; set; }
    }
}
