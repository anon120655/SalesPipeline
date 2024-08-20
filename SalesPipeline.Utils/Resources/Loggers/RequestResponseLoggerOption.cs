using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Loggers
{
	public class RequestResponseLoggerOption
	{
		public bool IsEnabled { get; set; }
		public string? Name { get; set; }
		public string? DateTimeFormat { get; set; }
		public string? ErrorToMail { get; set; }
	}
}
