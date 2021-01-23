using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Subject
    {
        public Subject()
        {
            Changes = new HashSet<Change>();
            Lessons = new HashSet<Lesson>();
            RoomSubjectSpecializations = new HashSet<RoomSubjectSpecialization>();
            StudyPlans = new HashSet<StudyPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StudySemester { get; set; }

        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<RoomSubjectSpecialization> RoomSubjectSpecializations { get; set; }
        public virtual ICollection<StudyPlan> StudyPlans { get; set; }
    }
}
