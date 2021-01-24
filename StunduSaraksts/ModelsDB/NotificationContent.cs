using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Paziņojuma saturs nevar būt tukšs.")]
        public string Text { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
