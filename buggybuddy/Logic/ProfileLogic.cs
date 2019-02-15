using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using web.buggybuddy.core.Models.Dbos;
using web.buggybuddy.core.Models.ViewModels;
using web.buggybuddy.core.Repositories;

namespace web.buggybuddy.core.Logic
{
	public class ProfileLogic : IProfileLogic
	{
		public string SetPicture(string path)
		{
			if (!Directory.Exists("wwwroot/" + path + "/profilePicture/"))
			{
				Directory.CreateDirectory("wwwroot/" + path + "/profilePicture/");
			}
			var directory = Directory.GetFiles("wwwroot/" + path + "/profilePicture/");

			if (directory.Any())
			{
				var input = directory.FirstOrDefault();
				return ".." + input.Substring(input.IndexOf('/'));
			}
			return "../images/default-profile.png";
		}
	}
}
