using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class StudyPlan
    {
        public int Id { get; set; }
        public int Class { get; set; }
        public int Subject { get; set; }
        public byte HoursPerWeek { get; set; }
        public int StudySemester { get; set; }

        public virtual Class ClassNavigation { get; set; }
        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
    }
}
