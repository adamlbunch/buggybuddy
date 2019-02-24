using System.Collections.Generic;

namespace buggybuddy.Models.ViewModels
{
    public class ProfileViewModel
    {
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string Interest { get; set; }
		public string ProfilePicture { get; set; }
		public string LastViewedProspect { get; set; }
		public string DataFolder { get; set; }
		public string Info { get; set; }
		public List<ProfileViewModel> Matches { get; set; }
    }
}
