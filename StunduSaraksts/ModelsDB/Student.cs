using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public int Class { get; set; }

        public virtual AspNetUser AccountNavigation { get; set; }
        public virtual Class ClassNavigation { get; set; }
    }
}
