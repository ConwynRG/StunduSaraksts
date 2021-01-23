using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class RoomSubjectSpecialization
    {
        public int Id { get; set; }
        public int Subject { get; set; }
        public int Room { get; set; }
        public int Specialization { get; set; }
        public int StudySemester { get; set; }

        public virtual Room RoomNavigation { get; set; }
        public virtual Specialization SpecializationNavigation { get; set; }
        public virtual StudySemester StudySemesterNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
    }
}
