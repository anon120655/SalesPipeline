using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Customers
{
	public class CustomerFilter : PagerFilter
	{
		public Guid id { get; set; }
		public int? idnumber { get; set; }
		public short? status { get; set; }
		public string? searchtxt { get; set; }
		public string? ids { get; set; }
		public List<string?>? Selecteds { get; set; }

		public string SetParameter(bool? isPage = null)
		{
			string? ParameterAll = String.Empty;

			if (isPage.HasValue && isPage.Value)
				ParameterAll = $"page={page}";

			ParameterAll += $"&pagesize={pagesize}";

			if (id != Guid.Empty)
				ParameterAll += $"&id={id}";

			if (idnumber > 0)
				ParameterAll += $"&idnumber={idnumber}";

			if (status.HasValue)
				ParameterAll += $"&status={status}";

			if (!String.IsNullOrEmpty(searchtxt))
				ParameterAll += $"&searchtxt={searchtxt}";

			if (Selecteds?.Count > 0)
			{
				string joined = string.Join(",", Selecteds);
				ParameterAll += $"&ids={joined}";
			}

			return ParameterAll;
		}
	}
}
