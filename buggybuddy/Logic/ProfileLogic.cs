using System.IO;
using System.Linq;

namespace buggybuddy.Logic
{
    public interface IProfileLogic
    {
        string SetPicture(string path);
    }
    public class ProfileLogic : IProfileLogic
	{
		public string SetPicture(string path)
		{
			if (!Directory.Exists("wwwroot/" + path + "/profilePicture/"))
			{
				Directory.CreateDirectory("wwwroot/" + path + "/profilePicture/");
			}
			var directory = Directory.GetFiles("wwwroot/" + path + "/profilePicture/");

		    if (!directory.Any()) return "../images/default-profile.png";

		    var input = directory.FirstOrDefault();
		    return ".." + input?.Substring(input.IndexOf('/'));
		}
	}
}
