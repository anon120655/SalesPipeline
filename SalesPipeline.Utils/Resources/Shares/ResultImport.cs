using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class ResultImport
	{
		public string? Name { get; set; }
		public bool Success { get; set; } = true;
		public string? errorMessage { get; set; }
	}
}
