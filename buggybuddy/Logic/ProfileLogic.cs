using System.IO;
using System.Linq;

namespace buggybuddy.Logic
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
