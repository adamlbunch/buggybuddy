using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using buggybuddy.Models.Dbos;
using buggybuddy.Models.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace buggybuddy.Repositories
{
    public interface IMatchesRepository
    {
        DataResponse<Match> CheckForMatch(ProfileViewModel profile);
        void AddMatch(string user, string prospect);
        IEnumerable<Match> GetMatches(ProfileViewModel user);

    }

    public class MatchesRepository : IMatchesRepository
    {
		private readonly IConfiguration _configuration;

		public MatchesRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private IDbConnection Connection => new SqlConnection(_configuration["ConnectionStrings:BuggyBuddy"]);

		public DataResponse<Match> CheckForMatch(ProfileViewModel profile)
		{
			const string sQuery = @"SELECT * FROM [dbo].[Match]
						WHERE [User] = @Prospect and [Prospect] = @User";

			List<Match> entries;
			using (Connection)
			{
				entries = Connection.Query<Match>(sQuery, new
				{
					Prospect = profile.LastViewedProspect,
					User = profile.UserName
				}).ToList();
			}

			if (entries.Any())
			{
				return new DataResponse<Match>
				{
					Model = entries.FirstOrDefault(),
					Success = true,
					Message = "It's a match!"
				};
			}

			return new DataResponse<Match>
			{
				Success = false,
				Message = "No match."
			};
		}

		public void AddMatch(string user, string prospect)
        {
        	const string sQuery = @"INSERT INTO [dbo].[Match]([User], [Prospect]) VALUES 
						(@User, @Prospect)";

            using (Connection)
			{
				Connection.Execute(sQuery, new
				{
					User = user,
					Prospect = prospect
				});
			}
        }

		public IEnumerable<Match> GetMatches(ProfileViewModel user)
		{
			var sQuery = @"SELECT * FROM [dbo].[Match]
						 WHERE [User] = @User";

			List<Match> userInUsers;
			using (Connection)
			{
				userInUsers = Connection.Query<Match>(sQuery, new
				{
					User = user.UserName
				}).ToList();
			}

			sQuery = @"SELECT * FROM [dbo].[Match]
					 WHERE [Prospect] = @User";

			List<Match> userInProspects;
			using (Connection)
			{
				userInProspects = Connection.Query<Match>(sQuery, new
				{
					User = user.UserName
				}).ToList();
			}

			var matches = new List<Match>();
			foreach (var userInProspect in userInProspects)
			{
				foreach (var userInUser in userInUsers)
				{
					if (userInProspect.User == userInUser.Prospect)
					{
						matches.Add(userInUser);
						continue;
					}
				}
			}

			if (matches.Any())
			{
				return matches;
			}

			return new List<Match>();
		}
    }
}
