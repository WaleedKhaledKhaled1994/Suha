using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
	public class WebApiLogs : BaseEntity
	{
		public string Message { get; set; }
		public string MessageTemplate { get; set; }
		public string Level { get; set; }
		public DateTime TimeStamp { get; set; }
		public string Exception { get; set; }
		public string Properties { get; set; }
		public string UserName { get; set; }
	}
}
