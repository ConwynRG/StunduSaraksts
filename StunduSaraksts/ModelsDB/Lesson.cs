using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Lesson
    {
        public Lesson()
        {
            Changes = new HashSet<Change>();
        }

        public int Id { get; set; }
        public int LessonTime { get; set; }
        public int Subject { get; set; }
        public int Class { get; set; }
        public int Teacher { get; set; }
        public int Room { get; set; }
        public int Day { get; set; }
        public int StudySemester { get; set; }

        public virtual Class ClassNavigation { get; set; }
        public virtual LessonTime LessonTimeNavigation { get; set; }
        public virtual Room RoomNavigation { get; set; }
        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
        public virtual Teacher TeacherNavigation { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
    }
}
