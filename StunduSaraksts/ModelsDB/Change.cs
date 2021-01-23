using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Change
    {
        public int Id { get; set; }
        public int Creator { get; set; }
        public int? Acceptor { get; set; }
        public int LessonTime { get; set; }
        public int Lesson { get; set; }
        public int Subject { get; set; }
        public int Class { get; set; }
        public int Teacher { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string RequestComment { get; set; }
        public string CommentForInvolved { get; set; }
        public string ReplyComment { get; set; }
        public bool? Accepted { get; set; }

        public virtual Teacher AcceptorNavigation { get; set; }
        public virtual Class ClassNavigation { get; set; }
        public virtual Teacher CreatorNavigation { get; set; }
        public virtual Lesson LessonNavigation { get; set; }
        public virtual LessonTime LessonTimeNavigation { get; set; }
        public virtual Room RoomNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
        public virtual Teacher TeacherNavigation { get; set; }
    }
}
