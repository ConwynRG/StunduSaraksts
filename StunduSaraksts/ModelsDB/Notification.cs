using System;
using System.Collections.Generic;

#nullable disable

namespace StunduSaraksts.ModelsDB
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public int Content { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime? ReceivedTime { get; set; }

        public virtual NotificationContent ContentNavigation { get; set; }
        public virtual AspNetUser RecipientNavigation { get; set; }
        public virtual AspNetUser SenderNavigation { get; set; }
    }
}
