using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class NotificationContent
    {
        public NotificationContent()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
