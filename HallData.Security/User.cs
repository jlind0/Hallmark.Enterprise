using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Security
{
    public class User
    {
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
