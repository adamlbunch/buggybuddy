using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.buggybuddy.core.Models.ViewModels;

namespace web.buggybuddy.core.Repositories
{
    public interface IUserRepository
    {
		UserResponse AttemptRegister(IndexViewModel model);
		UserResponse AttemptLogin(IndexViewModel model);
		UserResponse RequestRandomUser(ProfileViewModel model);
	}
}
