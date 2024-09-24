using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class XLookupRequest
	{
		//double lookupValue, List<XLookUpModel> lookUpModel, int searchMode = -1
		public double lookupValue { get; set; }
		public List<XLookUpModel> lookUpModel { get; set; } = new();
		public int match_mode { get; set; } = 1;
		public int search_mode { get; set; } = 1;
	}
}
