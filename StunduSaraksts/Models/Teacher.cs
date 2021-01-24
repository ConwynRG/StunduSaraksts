using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StunduSaraksts.ModelsDB
{
    public partial class Teacher
    {
        public string FullName
        {
            get
            {
                string str = "";
                str += AccountNavigation.Name + " ";
                if (AccountNavigation.SecondName != null)
                    str += AccountNavigation.SecondName + " ";
                str += AccountNavigation.Surname;
                return str;
            }
        }
    }
}
