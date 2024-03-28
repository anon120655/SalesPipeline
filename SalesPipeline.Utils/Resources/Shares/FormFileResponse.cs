using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class FormFileResponse
	{
		public Guid FileId { get; set; }

		public string? FileUrl { get; set; }

		public string? OriginalFileName { get; set; }
	}
}
