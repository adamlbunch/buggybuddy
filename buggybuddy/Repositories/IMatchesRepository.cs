using System.Collections.Generic;
using buggybuddy.Models.Dbos;
using buggybuddy.Models.ViewModels;

namespace buggybuddy.Repositories
{
    public interface IMatchesRepository
    {
		DataResponse<Match> CheckForMatch(ProfileViewModel profile);
		void AddMatch(string user, string prospect);
		IEnumerable<Match> GetMatches(ProfileViewModel user);

	}
}
