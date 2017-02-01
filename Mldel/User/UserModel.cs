using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mldel
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Pwd { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
    }
}
