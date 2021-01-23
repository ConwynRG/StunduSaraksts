using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Class
    {
        public Class()
        {
            Changes = new HashSet<Change>();
            Lessons = new HashSet<Lesson>();
            Students = new HashSet<Student>();
            StudyPlans = new HashSet<StudyPlan>();
        }

        public int Id { get; set; }
        public int Curator { get; set; }
        public string Name { get; set; }
        public int StudySemester { get; set; }

        public virtual Teacher CuratorNavigation { get; set; }
        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<StudyPlan> StudyPlans { get; set; }
    }
}
