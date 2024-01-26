using Microsoft.AspNetCore.WebUtilities;
using NPOI.SS.Formula.Eval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class allFilter : PagerFilter
	{
		public Guid id { get; set; }
		public int? idnumber { get; set; }
		public short? status { get; set; }
		public short? isshow { get; set; }
		public string? searchtxt { get; set; }
		public string? val1 { get; set; }
		public string? val2 { get; set; }
		public string? val3 { get; set; }
		public string? ids { get; set; }
		public string? sort { get; set; }
		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
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

			if (isshow.HasValue)
				ParameterAll += $"&isshow={isshow}";

			if (!String.IsNullOrEmpty(searchtxt))
				ParameterAll += $"&searchtxt={searchtxt}";

			if (!String.IsNullOrEmpty(val1))
				ParameterAll += $"&val1={val1}";

			if (!String.IsNullOrEmpty(val2))
				ParameterAll += $"&val2={val2}";

			if (!String.IsNullOrEmpty(val3))
				ParameterAll += $"&val3={val3}";

			if (!String.IsNullOrEmpty(sort))
				ParameterAll += $"&sort={sort}";

			if (Selecteds?.Count > 0)
			{
				string joined = string.Join(",", Selecteds);
				ParameterAll += $"&ids={joined}";
			}

			if (startdate.HasValue)
				ParameterAll += $"&startdate={GeneralUtils.DateToStrParameter(startdate)}";

			if (enddate.HasValue)
				ParameterAll += $"&enddate={GeneralUtils.DateToStrParameter(enddate)}";

			return ParameterAll;
		}

		public void SetUriQuery(string uriQuery)
		{
			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(page), out var _page))
				page = Convert.ToInt32(_page);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(pagesize), out var _pagesize))
				pagesize = Convert.ToInt32(_pagesize);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(status), out var _status))
				status = Convert.ToInt16(_status);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isshow), out var _isshow))
				isshow = Convert.ToInt16(_isshow);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(CurrentUserId), out var _CurrentUserId))
				CurrentUserId = Convert.ToInt32(_CurrentUserId);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(searchtxt), out var _searchtxt))
				searchtxt = _searchtxt;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val1), out var _val1))
				val1 = _val1;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val2), out var _val2))
				val2 = _val2;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val3), out var _val3))
				val3 = _val3;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(sort), out var _sort))
				sort = _sort;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("startdate", out var _startdate))
				startdate = GeneralUtils.DateNotNullToEn(_startdate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("enddate", out var _enddate))
				enddate = GeneralUtils.DateNotNullToEn(_enddate, "yyyy-MM-dd", Culture: "en-US");
		}


	}
}
