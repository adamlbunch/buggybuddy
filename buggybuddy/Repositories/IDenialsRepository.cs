using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Repositories
{
    public interface IDenialsRepository
    {
		void AddDenial(string user, string prospect);
	}
}
