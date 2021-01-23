using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class LessonTime
    {
        public LessonTime()
        {
            Changes = new HashSet<Change>();
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public int LessonNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int StudySemester { get; set; }

        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
