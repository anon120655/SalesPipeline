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
		public int? statussaleid { get; set; }
		public int? userid { get; set; }
		public int? assignuserid { get; set; }
		public short? status { get; set; }
		public short? isshow { get; set; }
		public string? searchtxt { get; set; }
		public string? juristicnumber { get; set; }
		public string? type { get; set; }
		public string? chain { get; set; }
		public string? businesstype { get; set; }
		public string? province { get; set; }
		public string? amphur { get; set; }
		public string? emp_id { get; set; }
		public string? emp_name { get; set; }
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

			if (statussaleid > 0)
				ParameterAll += $"&statussaleid={statussaleid}";

			if (assignuserid > 0)
				ParameterAll += $"&assignuserid={assignuserid}";

			if (status.HasValue)
				ParameterAll += $"&status={status}";

			if (isshow.HasValue)
				ParameterAll += $"&isshow={isshow}";

			if (!String.IsNullOrEmpty(searchtxt))
				ParameterAll += $"&searchtxt={searchtxt}";

			if (!String.IsNullOrEmpty(juristicnumber))
				ParameterAll += $"&juristicnumber={juristicnumber}";

			if (!String.IsNullOrEmpty(type))
				ParameterAll += $"&type={type}";

			if (!String.IsNullOrEmpty(chain))
				ParameterAll += $"&chain={chain}";

			if (!String.IsNullOrEmpty(businesstype))
				ParameterAll += $"&businesstype={businesstype}";

			if (!String.IsNullOrEmpty(province))
				ParameterAll += $"&province={province}";

			if (!String.IsNullOrEmpty(amphur))
				ParameterAll += $"&amphur={amphur}";

			if (!String.IsNullOrEmpty(emp_id))
				ParameterAll += $"&emp_id={emp_id}";

			if (!String.IsNullOrEmpty(emp_name))
				ParameterAll += $"&emp_name={emp_name}";

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

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(statussaleid), out var _statussaleid))
				statussaleid = Convert.ToInt16(_statussaleid);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(assignuserid), out var _assignuserid))
				assignuserid = Convert.ToInt16(_assignuserid);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(status), out var _status))
				status = Convert.ToInt16(_status);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isshow), out var _isshow))
				isshow = Convert.ToInt16(_isshow);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(CurrentUserId), out var _CurrentUserId))
				CurrentUserId = Convert.ToInt32(_CurrentUserId);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(searchtxt), out var _searchtxt))
				searchtxt = _searchtxt;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(juristicnumber), out var _juristicnumber))
				juristicnumber = _juristicnumber;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(chain), out var _chain))
				chain = _chain;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(type), out var _type))
				type = _type;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(businesstype), out var _businesstype))
				businesstype = _businesstype;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(province), out var _province))
				province = _province;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(amphur), out var _amphur))
				amphur = _amphur;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(emp_id), out var _emp_id))
				emp_id = _emp_id;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(emp_name), out var _emp_name))
				emp_name = _emp_name;

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
