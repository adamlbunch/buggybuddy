using System;
using System.Data;
using System.Data.SqlClient;
using buggybuddy.Models.Dbos;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace buggybuddy.Repositories
{
    public class DenialsRepository : IDenialsRepository
    {
		private readonly IConfiguration _configuration;

		public DenialsRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private IDbConnection Connection => new SqlConnection(_configuration["ConnectionStrings:BuggyBuddy"]);

		public void AddDenial(string user, string prospect)
		{
			var sQuery = @"INSERT INTO [dbo].[Denial]([User], [Prospect]) VALUES 
						(@User, @Prospect)";

			using (Connection)
			{
				Connection.Execute(sQuery, new
				{
					User = user,
					Prospect = prospect,
				});
			}
		}
	}

	public class DenialsResponse
	{
		public Denial Model { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
	}
}
