using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web.buggybuddy.core.Logic;
using web.buggybuddy.core.Models;
using web.buggybuddy.core.Models.Dbos;
using web.buggybuddy.core.Models.ViewModels;
using web.buggybuddy.core.Repositories;

namespace web.buggybuddy.core.Controllers
{
    public class HomeController : Controller
    {
		//"Server=(localdb)\\mssqllocaldb;Database=BuggyBuddy;Trusted_Connection=True;MultipleActiveResultSets=true"
		const string SessionKeyUserName = "_UserName";
		const string SessionKeyFirstName = "_FirstName";
		const string SessionKeyLastName = "_LastName";
		const string SessionKeyGender = "_Gender";
		const string SessionKeyInterest = "_Interest";
		const string SessionKeyProfilePicture = "_ProfilePicture";
		const string SessionKeyDataFolder = "_DataFolder";
		const string SessionKeyProspectPicture = "_ProspectPicture";
		const string SessionKeyLastViewedProspect = "_LastViewedProspect";
		const string SessionKeyInfo = "_Info";

		const string SessionKeyProspectUserName = "_ProspectUserName";
		const string SessionKeySearchProspectPicture = "_ProspectPicture";
		const string SessionKeyMatchSuccessMessage = "_MatchSuccessMessage";


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
				

			if (ModelState.IsValid)
			{
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

						string matchPath = "../images/default-profile.png";
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

			}
			return View("Index", model);
		}

		[HttpPost]
		public async Task<IActionResult> Register(IndexViewModel model, IFormFile picture)
		{
			HttpContext.Session.Clear();
			foreach (var key in ModelState.Keys.Where(m => m.StartsWith("Login")).ToList())
			{
				ModelState.Remove(key);
			}

			if (ModelState.IsValid)
			{

				
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

						string matchPath = "../images/default-profile.png";
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

			}
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

				string matchPath = "../images/default-profile.png";
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
				return View("Search", model);
			}

			var matches = new List<ProfileViewModel>();
			foreach (var match in _matchesRepository.GetMatches(currentUser))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

				string matchPath = "../images/default-profile.png";
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
			foreach (var match in _matchesRepository.GetMatches(currentUser))
			{
				var directory = Directory.GetFiles("wwwroot/data/" + match.Prospect + "/profilePicture/");

				string matchPath = "../images/default-profile.png";
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

				string matchPath = "../images/default-profile.png";
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

		public IActionResult ProfileTest()
		{
			return View();
		}

		public IActionResult SearchTest()
		{
			return View();
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
