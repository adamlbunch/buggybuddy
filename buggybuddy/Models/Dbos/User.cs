using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Models.Dbos
{
    public class User
    {
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
		public string Gender { get; set; }
		public string Interest { get; set; }
		public string DataFolder { get; set; }
		public string Info { get; set; }
    }
}
