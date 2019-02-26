using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using buggybuddy.Logic;
using buggybuddy.Models.Dbos;
using buggybuddy.Models.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace buggybuddy.Repositories
{
    public interface IUserRepository
    {
        UserResponse AttemptRegister(IndexViewModel model);
        UserResponse AttemptLogin(IndexViewModel model);
        UserResponse RequestRandomUser(ProfileViewModel model);
    }
    public class UserRepository : IUserRepository
    {
		private readonly IConfiguration _configuration;
		private readonly IProfileLogic _profileLogic;

		public UserRepository(IConfiguration configuration,
			IProfileLogic profileLogic)
		{
			_configuration = configuration;
			_profileLogic = profileLogic;
		}

		private IDbConnection Connection => new SqlConnection(_configuration["ConnectionStrings:BuggyBuddy"]);

		public UserResponse AttemptRegister(IndexViewModel model)
		{
			var user = new User
			{
				FirstName = model.RegisterFirstName,
				LastName = model.RegisterLastName,
				UserName = model.RegisterUserName,
				Gender = model.RegisterGender,
				Interest = model.RegisterInterest,
				Password = model.RegisterPassword,
				Info = model.RegisterInfo
			};

            var sQuery = "SELECT * FROM [dbo].[User] WHERE [UserName] = @UserName";
            List<User> entries;
			using (Connection)
			{
				entries = Connection.Query<User>(sQuery, new
				{
					user.UserName
				}).ToList();
			}

			if (entries.Any())
			{
				return new UserResponse
				{
					Success = false,
					Message = "Username already Exists"
				};
			}

			var root = Path.Combine(Environment.CurrentDirectory, "wwwroot/data");
			var userFolder = root + "/" + model.RegisterUserName;
			var profilePictureFolder = userFolder + "/profilePicture/";

			if (!Directory.Exists(root))
			{
				Directory.CreateDirectory(root);
			}

			if (!Directory.Exists(userFolder))
			{
				Directory.CreateDirectory(userFolder);
			}

			if (!Directory.Exists(profilePictureFolder))
			{
				Directory.CreateDirectory(profilePictureFolder);
			}

			user.DataFolder = "data/" + model.RegisterUserName;

			sQuery = @"INSERT INTO [dbo].[User]([UserName], [FirstName], [LastName], [Password], [Gender], [Interest], [DataFolder], [Info]) VALUES 
						(@UserName, @FirstName, @LastName, @Password, @Gender, @Interest, @DataFolder, @Info)";
			using (Connection)
			{
				Connection.Execute(sQuery, new
				{
					UserName = model.RegisterUserName,
					FirstName = model.RegisterFirstName,
					LastName = model.RegisterLastName,
					Password = model.RegisterPassword,
					Gender = model.RegisterGender,
					Interest = model.RegisterInterest,
					DataFolder = user.DataFolder,
					Info = model.RegisterInfo
				});
			}

			return new UserResponse
			{
				Model = user,
				Success = true,
				Message = "User successfully created"
			};
		}

		public UserResponse AttemptLogin(IndexViewModel model)
		{
			const string sQuery = "SELECT * FROM [dbo].[User] WHERE [UserName] = @UserName and [Password] = @Password";
			var user = new User
			{
				UserName = model.LoginUserName,
				Password = model.LoginPassword
			};

			List<User> entries;
			using (Connection)
			{
				entries = Connection.Query<User>(sQuery, new
				{
					user.UserName,
					user.Password
				}).ToList();
			}

			if (entries.Any())
			{
				return new UserResponse
				{
					Model = entries.FirstOrDefault(),
					Success = true,
					Message = "Successfully logged in"
				};
			}

			return new UserResponse
			{
				Success = false,
				Message = "Incorrect username or password"
			};
		}

		public UserResponse RequestRandomUser(ProfileViewModel model)
		{
			var sQuery = @"SELECT * FROM [dbo].[User] WHERE
							[UserName] != @UserName AND [Gender] = @Interest
							ORDER BY NEWID()";

			List<User> users;
			using (Connection)
			{
				users = Connection.Query<User>(sQuery, new
				{
					UserName = model.UserName,
					Interest = model.Interest
				}).ToList();
			}

		    if (!users.Any())
		        return new UserResponse
		        {
		            Success = false,
		            Message = "There are currently no other users in your area"
		        };

		    sQuery = @"SELECT * FROM [dbo].[Denial] WHERE
					[User] = @UserName";

		    List<Denial> denials;
		    using (Connection)
		    {
		        denials = Connection.Query<Denial>(sQuery, new
		        {
		            UserName = model.UserName
		        }).ToList();
		    }

		    sQuery = @"SELECT * FROM [dbo].[Match] WHERE
					[User] = @UserName";

		    List<Match> matches;
		    using (Connection)
		    {
		        matches = Connection.Query<Match>(sQuery, new
		        {
		            UserName = model.UserName
		        }).ToList();
		    }

		    var approved = new List<User>(users);
		    foreach (var denial in denials)
		    {
		        foreach (var user in users)
		        {
		            if (denial.Prospect == user.UserName)
		            {
		                approved.Remove(user);
		            }
		        }
		    }

		    foreach (var match in matches)
		    {
		        foreach (var user in users)
		        {
		            if (match.Prospect == user.UserName)
		            {
		                approved.Remove(user);
		            }
		        }
		    }

		    if (approved.Any())
		    {
		        return new UserResponse
		        {
		            Model = approved.FirstOrDefault(),
		            Success = true,
		            Message = "Successfully found a random user"
		        };
		    }

		    /*if (users.Any())
			{
				return new UserResponse
				{
					Model = entries.FirstOrDefault(),
					Success = true,
					Message = "Successfully found a random user."
				};
			}*/
			
			return new UserResponse
			{
				Success = false,
				Message = "There are currently no other users in your area"
			};
		}
	}

	public class UserResponse
	{
		public User Model { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
	}
}
