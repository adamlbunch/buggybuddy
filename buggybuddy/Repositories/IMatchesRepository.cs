using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.buggybuddy.core.Models.Dbos;
using web.buggybuddy.core.Models.ViewModels;

namespace web.buggybuddy.core.Repositories
{
    public interface IMatchesRepository
    {
		DataResponse<Match> CheckForMatch(ProfileViewModel profile);
		void AddMatch(string user, string prospect);
		List<Match> GetMatches(ProfileViewModel user);

	}
}
