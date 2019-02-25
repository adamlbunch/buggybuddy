using buggybuddy.Logic;
using buggybuddy.Repositories;
using StructureMap;

namespace buggybuddy
{
    public class DependencyRegistry : Registry
    {
		public DependencyRegistry()
		{
			For<IUserRepository>().Add<UserRepository>();
			For<IDenialsRepository>().Add<DenialsRepository>();
			For<IMatchesRepository>().Add<MatchesRepository>();
			For<IProfileLogic>().Add<ProfileLogic>();
		}
    }
}
