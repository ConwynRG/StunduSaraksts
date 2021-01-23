using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace StunduSaraksts.ModelsDB
{
    public partial class AspNetUser
    {
        public bool isStudent()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Students.Where(s => s.Account == this.Id).Any();
            }
        }

        public bool isTeacher()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Teachers.Where(s => s.Account == this.Id).Any();
            }
        }

        public bool isAdmin()
        {
            using (var context = new StunduSarakstsContext())
            {
                return context.Teachers.Where(s => (s.Account == this.Id) && (s.IsAdmin==true)).Any();
            }
        }
    }
}
