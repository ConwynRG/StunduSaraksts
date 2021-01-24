using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StunduSaraksts.Models
{
    public class ReservationAdminForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Komentāra lauks nevar būt tukšš")]
        public string ReplyComment { get; set; }
        public bool Accepted { get; set; }
    }
}
