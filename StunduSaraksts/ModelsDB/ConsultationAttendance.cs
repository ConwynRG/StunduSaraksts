using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class ConsultationAttendance
    {
        public int Student { get; set; }
        public int Consultation { get; set; }
        public string Comment { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool Attends { get; set; }

        public virtual Consultation ConsultationNavigation { get; set; }
        public virtual Student StudentNavigation { get; set; }
    }
}
