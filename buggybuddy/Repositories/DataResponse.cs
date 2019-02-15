using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.buggybuddy.core.Repositories
{
    public class DataResponse<T>
    {
		public T Model { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
    }
}
