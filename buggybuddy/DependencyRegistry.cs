using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using buggybuddy.Logic;
using buggybuddy.Repositories;

namespace web.buggybuddy.core
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
