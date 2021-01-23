using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Specialization
    {
        public Specialization()
        {
            RoomSubjectSpecializations = new HashSet<RoomSubjectSpecialization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RoomSubjectSpecialization> RoomSubjectSpecializations { get; set; }
    }
}
