using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using buggybuddy.Logic;
using buggybuddy.Models;
using buggybuddy.Models.ViewModels;
using buggybuddy.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace buggybuddy.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKeyUserName = "_UserName";
        private const string SessionKeyFirstName = "_FirstName";
        private const string SessionKeyLastName = "_LastName";
        public const string SessionKeyGender = "_Gender";
        public const string SessionKeyInterest = "_Interest";
        public const string SessionKeyProfilePicture = "_ProfilePicture";
        private const string SessionKeyDataFolder = "_DataFolder";
        public const string SessionKeyProspectPicture = "_ProspectPicture";
        public const string SessionKeyLastViewedProspect = "_LastViewedProspect";
        private const string SessionKeyInfo = "_Info";

        public const string SessionKeyProspectUserName = "_ProspectUserName";
        public const string SessionKeySearchProspectPicture = "_ProspectPicture";
        public const string SessionKeyMatchSuccessMessage = "_MatchSuccessMessage";


		private readonly IUserRepository _userRepository;
		private readonly IDenialsRepository _denialsRepository;
		private readonly IMatchesRepository _matchesRepository;
		private readonly IProfileLogic _profileLogic;

		public HomeController(IUserRepository userRepository,
			IDenialsRepository denialsRepository,
			IMatchesRepository matchesRepository,
			IProfileLogic profileLogic)
		{
			_userRepository = userRepository;
			_denialsRepository = denialsRepository;
			_matchesRepository = matchesRepository;
			_profileLogic = profileLogic;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return View("Index");
		}

		[HttpPost]
		public IActionResult Login(IndexViewModel model)
		{
			HttpContext.Session.Clear();
			foreach (var key in ModelState.Keys.Where(m => m.StartsWith("Register")).ToList())
			{
				ModelState.Remove(key);
			}

		    if (!ModelState.IsValid) return View("Index", model);

		    var response = _userRepository.AttemptLogin(model);
		    if (response.Success)
		    {
		        var profileModel = new ProfileViewModel
		        {
		            UserName = response.Model.UserName,
		            FirstName = response.Model.FirstName,
		            LastName = response.Model.LastName,
		            Gender = response.Model.Gender,
		            Interest = response.Model.Interest,
		            ProfilePicture = _profileLogic.SetPicture(response.Model.DataFolder),
		            LastViewedProspect = "",
		            DataFolder = response.Model.DataFolder,
		            Info = response.Model.Info
		        };

		        var matches = new List<ProfileViewModel>();
		        foreach (var match in _matchesRepository.GetMatches(profileModel))
		        {
		            var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

		            var matchPath = "../images/default-profile.png";
		            if (directory.Any())
		            {
		                matchPath = directory.FirstOrDefault();
		                matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
		            }

		            matches.Add(new ProfileViewModel
		            {
		                UserName = match.Prospect,
		                ProfilePicture = matchPath
		            });
		        }
		        profileModel.Matches = matches;

		        SetSessionVariables(profileModel);
		        return View("Profile", profileModel);
		    }

		    ModelState.AddModelError(string.Empty, response.Message);
		    return View("Index", model);
		}

		[HttpPost]
		public async Task<IActionResult> Register(IndexViewModel model, IFormFile picture)
		{
			HttpContext.Session.Clear();

            // This is here because we are attempting to make a single page Login/Register system, which doesn't play well with the MVC pattern
			foreach (var key in ModelState.Keys.Where(m => m.StartsWith("Login")).ToList())
			{
				ModelState.Remove(key);
			}

		    if (!ModelState.IsValid) return View("Index", model);
        
		    var response = _userRepository.AttemptRegister(model);
		    if (response.Success)
		    {
		        if (picture != null)
		        {
		            var picturePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + response.Model.DataFolder + "\\profilePicture", picture.FileName);

		            using (var stream = new FileStream(picturePath, FileMode.Create))
		            {
		                await picture.CopyToAsync(stream);
		            }
		        }

		        var profileModel = new ProfileViewModel
		        {
		            UserName = response.Model.UserName,
		            FirstName = response.Model.FirstName,
		            LastName = response.Model.LastName,
		            Gender = response.Model.Gender,
		            Interest = response.Model.Interest,
		            ProfilePicture = _profileLogic.SetPicture(response.Model.DataFolder),
		            LastViewedProspect = "",
		            DataFolder = response.Model.DataFolder,
		            Info = response.Model.Info,
		        };

		        var matches = new List<ProfileViewModel>();
		        foreach (var match in _matchesRepository.GetMatches(profileModel))
		        {
		            var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

		            var matchPath = "../images/default-profile.png";
		            if (directory.Any())
		            {
		                matchPath = directory.FirstOrDefault();
		                matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
		            }

		            matches.Add(new ProfileViewModel
		            {
		                UserName = match.Prospect,
		                ProfilePicture = matchPath
		            });
		        }
		        profileModel.Matches = matches;

		        SetSessionVariables(profileModel);
		        return View("Profile", profileModel);
		    }

		    ModelState.AddModelError(string.Empty, response.Message);
		    return View("Index", model);
		}

		[HttpGet]
		public IActionResult Profile()
		{
			var model = GetSessionVariables();

			var matches = new List<ProfileViewModel>();
			foreach (var match in _matchesRepository.GetMatches(model))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

				var matchPath = "../images/default-profile.png";
				if (directory.Any())
				{
					matchPath = directory.FirstOrDefault();
					matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
				}

				matches.Add(new ProfileViewModel
				{
					UserName = match.Prospect,
					ProfilePicture = matchPath
				});
			}
			model.Matches = matches;

			return View(model);
		}

		[HttpPost]
		public IActionResult Search()
		{
			var model = new SearchViewModel();
			var currentUser = GetSessionVariables();
			var randomUser = _userRepository.RequestRandomUser(currentUser);
			if (randomUser.Success)
			{
				model.ProspectUserName = randomUser.Model.UserName;
				model.ProspectPicture = _profileLogic.SetPicture(randomUser.Model.DataFolder);
				model.ProspectInfo = randomUser.Model.Info;
				HttpContext.Session.SetString(SessionKeyLastViewedProspect, randomUser.Model.UserName);
				return View(model);
			}

			var matches = new List<ProfileViewModel>();
			foreach (var match in _matchesRepository.GetMatches(currentUser))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

				var matchPath = "../images/default-profile.png";
				if (directory.Any())
				{
					matchPath = directory.FirstOrDefault();
					matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
				}

				matches.Add(new ProfileViewModel
				{
					UserName = match.Prospect,
					ProfilePicture = matchPath
				});
			}
			currentUser.Matches = matches;

			model.ProspectUserName = "";
			model.ProspectPicture = "../images/no-matches-available.png";
			HttpContext.Session.SetString(SessionKeyLastViewedProspect, "");
			ModelState.AddModelError(string.Empty, randomUser.Message);
			return View("Profile", currentUser);
		}

		[HttpPost]
		public IActionResult BackToProfile()
		{
			return RedirectToAction("Profile");
		}

		[HttpPost]
		public IActionResult Deny()
		{
			var model = new SearchViewModel();
			var currentUser = GetSessionVariables();
			if (currentUser.LastViewedProspect != "")
			{
				_denialsRepository.AddDenial(currentUser.UserName, currentUser.LastViewedProspect);
			}

			var randomUser = _userRepository.RequestRandomUser(currentUser);
			if (randomUser.Success)
			{
				model.ProspectUserName = randomUser.Model.UserName;
				model.ProspectPicture = _profileLogic.SetPicture(randomUser.Model.DataFolder);
				model.ProspectInfo = randomUser.Model.Info;
				HttpContext.Session.SetString(SessionKeyLastViewedProspect, randomUser.Model.UserName);

				HttpContext.Session.SetString(SessionKeyProspectUserName, model.ProspectUserName);
				HttpContext.Session.SetString(SessionKeyProspectPicture, model.ProspectPicture);
				
				return RedirectToAction("Denied");
			}

			var matches = new List<ProfileViewModel>();
		    var matchPath = "../images/default-profile.png";
		    foreach (var match in _matchesRepository.GetMatches(currentUser))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

			    if (directory.Any())
				{
					matchPath = directory.FirstOrDefault();
					matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
				}

				matches.Add(new ProfileViewModel
				{
					UserName = match.Prospect,
					ProfilePicture = matchPath
				});
			}
			currentUser.Matches = matches;

			model.ProspectUserName = "";
			model.ProspectPicture = "../images/no-matches-available.png";
			HttpContext.Session.SetString(SessionKeyLastViewedProspect, "");
			ModelState.AddModelError(string.Empty, randomUser.Message);
			return View("Profile", currentUser);
		}

		[HttpGet]
		public IActionResult Denied()
		{
			var model = new SearchViewModel
			{
				ProspectUserName = HttpContext.Session.GetString(SessionKeyProspectUserName),
				ProspectPicture = HttpContext.Session.GetString(SessionKeySearchProspectPicture),
				MatchSuccessMessage = HttpContext.Session.GetString(SessionKeyMatchSuccessMessage)
			};

			return View("Search", model);
		}

		[HttpPost]
		public IActionResult Match()
		{
			var model = new SearchViewModel();
			var currentUser = GetSessionVariables();
			if (currentUser.LastViewedProspect != "")
			{
				_matchesRepository.AddMatch(currentUser.UserName, currentUser.LastViewedProspect);
			}

			var matchResponse = _matchesRepository.CheckForMatch(currentUser);
			if (matchResponse.Success)
			{
				model.MatchSuccessMessage = matchResponse.Message;
			}

			var randomUser = _userRepository.RequestRandomUser(currentUser);
			if (randomUser.Success)
			{
				model.ProspectUserName = randomUser.Model.UserName;
				model.ProspectPicture = _profileLogic.SetPicture(randomUser.Model.DataFolder);
				model.ProspectInfo = randomUser.Model.Info;
				HttpContext.Session.SetString(SessionKeyLastViewedProspect, randomUser.Model.UserName);
				return View("Search", model);
			}

			var matches = new List<ProfileViewModel>();
			foreach (var match in _matchesRepository.GetMatches(currentUser))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

				var matchPath = "../images/default-profile.png";
				if (directory.Any())
				{
					matchPath = directory.FirstOrDefault();
					matchPath = ".." + matchPath.Substring(matchPath.IndexOf('/'));
				}

				matches.Add(new ProfileViewModel
				{
					UserName = match.Prospect,
					ProfilePicture = matchPath
				});
			}
			currentUser.Matches = matches;

			model.ProspectUserName = "";
			model.ProspectPicture = "../images/no-matches-available.png";
			HttpContext.Session.SetString(SessionKeyLastViewedProspect, "");
			ModelState.AddModelError(string.Empty, randomUser.Message);
			return View("Profile", currentUser);
		}

		[HttpGet]
		public IActionResult Matched()
		{
			var model = new SearchViewModel
			{
				ProspectUserName = HttpContext.Session.GetString(SessionKeyProspectUserName),
				ProspectPicture = HttpContext.Session.GetString(SessionKeySearchProspectPicture),
				MatchSuccessMessage = HttpContext.Session.GetString(SessionKeyMatchSuccessMessage)
			};

			return View("Search", model);
		}

		public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		private void SetSessionVariables(ProfileViewModel model)
		{
			HttpContext.Session.SetString(SessionKeyUserName, model.UserName);
			HttpContext.Session.SetString(SessionKeyFirstName, model.FirstName);
			HttpContext.Session.SetString(SessionKeyLastName, model.LastName);
			HttpContext.Session.SetString(SessionKeyGender, model.Gender);
			HttpContext.Session.SetString(SessionKeyInterest, model.Interest);
			HttpContext.Session.SetString(SessionKeyProfilePicture, model.ProfilePicture);
			HttpContext.Session.SetString(SessionKeyDataFolder, model.DataFolder);
			HttpContext.Session.SetString(SessionKeyLastViewedProspect, model.LastViewedProspect);
			HttpContext.Session.SetString(SessionKeyInfo, model.Info);
		}

		private ProfileViewModel GetSessionVariables()
		{
			return new ProfileViewModel
			{
				UserName = HttpContext.Session.GetString(SessionKeyUserName),
				FirstName = HttpContext.Session.GetString(SessionKeyFirstName),
				LastName = HttpContext.Session.GetString(SessionKeyLastName),
				Gender = HttpContext.Session.GetString(SessionKeyGender),
				Interest = HttpContext.Session.GetString(SessionKeyInterest),
				ProfilePicture = HttpContext.Session.GetString(SessionKeyProfilePicture),
				DataFolder = HttpContext.Session.GetString(SessionKeyDataFolder),
				LastViewedProspect = HttpContext.Session.GetString(SessionKeyLastViewedProspect),
				Info = HttpContext.Session.GetString(SessionKeyInfo)
			};
		}
    }
}
