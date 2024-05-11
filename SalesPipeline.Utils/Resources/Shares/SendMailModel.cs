using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class SendMailModel
	{
		public string? Body { get; set; }
		public string? Email { get; set; }
		public string? Subject { get; set; }
		public string? Template { get; set; }
		public List<string>? CcList { get; set; }
	}
}
