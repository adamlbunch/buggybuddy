using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Models
{
    public class UserSession
    {
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string Interest { get; set; }
		public string ProfilePicture { get; set; }
		public string DataFolder { get; set; }
		public string ProspectPicture { get; set; }
		public string LastViewedProspect { get; set; }
		public string CurrentPropsectUserName { get; set; }
		public string CurrentProspectPicture { get; set; }
		public string MatchSuccessMessage { get; set; }
    }
}
