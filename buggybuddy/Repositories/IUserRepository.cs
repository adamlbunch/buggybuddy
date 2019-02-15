using buggybuddy.Models.ViewModels;
using web.buggybuddy.core.Models.ViewModels;

namespace buggybuddy.Repositories
{
    public interface IUserRepository
    {
		UserResponse AttemptRegister(IndexViewModel model);
		UserResponse AttemptLogin(IndexViewModel model);
		UserResponse RequestRandomUser(ProfileViewModel model);
	}
}
