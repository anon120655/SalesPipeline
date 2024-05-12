using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class SendMailModel
	{
		public int? CurrentUserId { get; set; }
		public string? Body { get; set; }
		public string? Email { get; set; }
		public string? Subject { get; set; }
		public Guid? TemplateId { get; set; }
		public List<string>? CcList { get; set; }
	}
}
