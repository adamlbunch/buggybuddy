using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Models.Dbos
{
    public class Denial
    {
		public string User { get; set; }
		public string Prospect { get; set; }
		public DateTime? LastChecked { get; set; }
    }
}
