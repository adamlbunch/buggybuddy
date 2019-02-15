using System.Collections.Generic;
using buggybuddy.Models.Dbos;
using web.buggybuddy.core.Models.ViewModels;

namespace buggybuddy.Repositories
{
    public interface IMatchesRepository
    {
		DataResponse<Match> CheckForMatch(ProfileViewModel profile);
		void AddMatch(string user, string prospect);
		List<Match> GetMatches(ProfileViewModel user);

	}
}
