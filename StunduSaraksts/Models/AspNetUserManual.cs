using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace StunduSaraksts.ModelsDB
{
    public partial class AspNetUser
    {

        /*public bool IsStudent()
        {
            return this.Students.Count > 0;
        }*/

        public bool IsStudent()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Students.Where(s => s.Account == this.Id).Any();
            }
        }

        /*public Student GetStudent()
        {
            return this.Students.FirstOrDefault();
        }*/

        public Student GetStudent()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Students.Where(s => s.Account == this.Id).FirstOrDefault();
            }
        }

        /*public bool IsTeacher()
        {
            return this.Teachers.Count > 0;
        }*/

        public bool IsTeacher()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Teachers.Where(s => s.Account == this.Id).Any();
            }
        }

        /*public Teacher GetTeacher()
        {
            return this.Teachers.FirstOrDefault();
        }
*/
        public Teacher GetTeacher()
        {
            using (StunduSarakstsContext context = new StunduSarakstsContext())
            {
                return context.Teachers.Where(s => s.Account == this.Id).FirstOrDefault();
            }
        }

        /*public bool IsAdmin()
        {
            return IsTeacher() && GetTeacher().IsAdmin==true;
        }*/

        public bool IsAdmin()
        {
            using (var context = new StunduSarakstsContext())
            {
                return context.Teachers.Where(s => (s.Account == this.Id) && (s.IsAdmin==true)).Any();
            }
        }
    }
}
