using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Teacher
    {
        public Teacher()
        {
            ChangeAcceptorNavigations = new HashSet<Change>();
            ChangeCreatorNavigations = new HashSet<Change>();
            ChangeTeacherNavigations = new HashSet<Change>();
            Classes = new HashSet<Class>();
            Consultations = new HashSet<Consultation>();
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime? WorkStart { get; set; }
        public DateTime? WorkEnd { get; set; }

        public virtual AspNetUser AccountNavigation { get; set; }
        public virtual ICollection<Change> ChangeAcceptorNavigations { get; set; }
        public virtual ICollection<Change> ChangeCreatorNavigations { get; set; }
        public virtual ICollection<Change> ChangeTeacherNavigations { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
