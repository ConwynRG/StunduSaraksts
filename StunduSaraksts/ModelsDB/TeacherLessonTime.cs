using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class TeacherLessonTime
    {
        public int Teacher { get; set; }
        public int Day { get; set; }
        public int LessonTime { get; set; }

        public virtual LessonTime LessonTimeNavigation { get; set; }
        public virtual Teacher TeacherNavigation { get; set; }
    }
}
