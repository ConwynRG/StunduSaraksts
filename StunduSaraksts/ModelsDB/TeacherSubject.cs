using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class TeacherSubject
    {
        public int Id { get; set; }
        public int Teacher { get; set; }
        public int Subject { get; set; }
        public int StudySemester { get; set; }

        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
        public virtual Teacher TeacherNavigation { get; set; }
    }
}
