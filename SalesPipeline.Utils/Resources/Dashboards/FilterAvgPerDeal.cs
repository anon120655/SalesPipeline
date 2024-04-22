using NPOI.SS.Formula.Functions;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class FilterAvgPerDeal : PagerFilter
	{
		public int? userid { get; set; }
		public List<SelectModel>? DepartmentBranch { get; set; }
		public List<SelectModel>? Branchs { get; set; }
		public List<SelectModel>? RMUser { get; set; }

		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
		public string SetParameter(bool? isPage = null)
		{
			string? ParameterAll = String.Empty;

			if (isPage.HasValue && isPage.Value)
				ParameterAll = $"page={page}";

			ParameterAll += $"&pagesize={pagesize}";

			if (userid > 0)
				ParameterAll += $"&userid={userid}";

			if (DepartmentBranch?.Count > 0)
			{
				string joined = string.Join(",", DepartmentBranch);
				ParameterAll += $"&ids={joined}";
			}

			if (Branchs?.Count > 0)
			{
				string joined = string.Join(",", Branchs);
				ParameterAll += $"&ids2={joined}";
			}

			if (RMUser?.Count > 0)
			{
				string joined = string.Join(",", RMUser);
				ParameterAll += $"&ids3={joined}";
			}

			if (startdate.HasValue)
				ParameterAll += $"&startdate={GeneralUtils.DateToStrParameter(startdate)}";

			if (enddate.HasValue)
				ParameterAll += $"&enddate={GeneralUtils.DateToStrParameter(enddate)}";

			return ParameterAll;
		}
	}
}
