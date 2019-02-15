using System;

namespace buggybuddy.Models.Dbos
{
    public class Denial
    {
		public string User { get; set; }
		public string Prospect { get; set; }
		public DateTime? LastChecked { get; set; }
    }
}
