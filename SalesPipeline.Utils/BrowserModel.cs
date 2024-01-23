using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public class BrowserModel
	{
		public string userAgent { get; set; } = null!;
		public Regex OS { get; set; } = null!;
		public Regex device { get; set; } = null!;
	}
}
