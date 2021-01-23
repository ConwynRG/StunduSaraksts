using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class StudySemester
    {
        public StudySemester()
        {
            Classes = new HashSet<Class>();
            LessonTimes = new HashSet<LessonTime>();
            Lessons = new HashSet<Lesson>();
            RoomSubjectSpecializations = new HashSet<RoomSubjectSpecialization>();
            Rooms = new HashSet<Room>();
            StudyPlans = new HashSet<StudyPlan>();
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<LessonTime> LessonTimes { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<RoomSubjectSpecialization> RoomSubjectSpecializations { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<StudyPlan> StudyPlans { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
