﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using buggybuddy.Models.Dbos;
using buggybuddy.Models.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace buggybuddy.Repositories
{
    public class MatchesRepository : IMatchesRepository
    {
		private readonly IConfiguration _configuration;

		public MatchesRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private IDbConnection connection => new SqlConnection(_configuration["ConnectionStrings:BuggyBuddy"]);

		public DataResponse<Match> CheckForMatch(ProfileViewModel profile)
		{
			var sQuery = @"SELECT * FROM [dbo].[Matches]
						WHERE [User] = @Prospect and [Prospect] = @User";

			var entries = new List<Match>();
			using (connection)
			{
				entries = connection.Query<Match>(sQuery, new
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
			var sQuery = @"INSERT INTO [dbo].[Matches]([User], [Prospect]) VALUES 
						(@User, @Prospect)";

			using (connection)
			{
				connection.Execute(sQuery, new
				{
					User = user,
					Prospect = prospect
				});
			}
		}

		public List<Match> GetMatches(ProfileViewModel user)
		{
			var sQuery = @"SELECT * FROM [dbo].[Matches]
						 WHERE [User] = @User";

			var userInUsers = new List<Match>();
			using (connection)
			{
				userInUsers = connection.Query<Match>(sQuery, new
				{
					User = user.UserName
				}).ToList();
			}

			sQuery = @"SELECT * FROM [dbo].[Matches]
					 WHERE [Prospect] = @User";

			var userInProspects = new List<Match>();
			using (connection)
			{
				userInProspects = connection.Query<Match>(sQuery, new
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
