using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using web.buggybuddy.core.Logic;
using web.buggybuddy.core.Models.Dbos;

namespace web.buggybuddy.core.Repositories
{
    public class DenialsRepository : IDenialsRepository
    {
		private readonly IConfiguration _configuration;

		public DenialsRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private IDbConnection connection => new SqlConnection(_configuration["ConnectionStrings:BuggyBuddy"]);

		public void AddDenial(string user, string prospect)
		{
			var sQuery = @"INSERT INTO [dbo].[Denials]([User], [Prospect], [LastChecked]) VALUES 
						(@User, @Prospect, @LastChecked)";

			using (connection)
			{
				connection.Execute(sQuery, new
				{
					User = user,
					Prospect = prospect,
					LastChecked = DateTime.Now
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
