using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Models.ViewModels
{
    public class SearchViewModel
    {
		public string ProspectUserName { get; set; }
		public string ProspectPicture { get; set; }
		public string ProspectInfo { get; set; }
		public string MatchSuccessMessage { get; set; }
	}
}
