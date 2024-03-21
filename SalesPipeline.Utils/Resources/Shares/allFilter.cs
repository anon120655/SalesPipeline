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
		public Guid? customerid { get; set; }
		public int? statussaleid { get; set; }
		public int? userid { get; set; }
		public int? assigncenter { get; set; }
		public int? assignrm { get; set; }
		public short? status { get; set; }
		public short? isshow { get; set; }
		public string? cif { get; set; }
		public string? searchtxt { get; set; }
		public string? juristicnumber { get; set; }
		public string? type { get; set; }
		public string? chain { get; set; }
		public string? isiccode { get; set; }
		public string? businesstype { get; set; }
		public int? provinceid { get; set; }
		public int? amphurid { get; set; }
		public string? province_name { get; set; }
		public string? amphur_name { get; set; }
		public string? branch { get; set; }
		public string? emp_id { get; set; }
		public string? emp_name { get; set; }
		public string? assignmentid { get; set; }
		public string? mcenter_code { get; set; }
		public string? mcenter_name { get; set; }
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

			if (customerid.HasValue && customerid != Guid.Empty)
				ParameterAll += $"&customerid={customerid}";

			if (statussaleid > 0)
				ParameterAll += $"&statussaleid={statussaleid}";

			if (assigncenter > 0)
				ParameterAll += $"&assigncenter={assigncenter}";

			if (assignrm > 0)
				ParameterAll += $"&assignrm={assignrm}";

			if (status.HasValue)
				ParameterAll += $"&status={status}";

			if (isshow.HasValue)
				ParameterAll += $"&isshow={isshow}";

			if (!String.IsNullOrEmpty(cif))
				ParameterAll += $"&cif={cif}";

			if (!String.IsNullOrEmpty(searchtxt))
				ParameterAll += $"&searchtxt={searchtxt}";

			if (!String.IsNullOrEmpty(juristicnumber))
				ParameterAll += $"&juristicnumber={juristicnumber}";

			if (!String.IsNullOrEmpty(type))
				ParameterAll += $"&type={type}";

			if (!String.IsNullOrEmpty(chain))
				ParameterAll += $"&chain={chain}";

			if (!String.IsNullOrEmpty(isiccode))
				ParameterAll += $"&isiccode={isiccode}";

			if (!String.IsNullOrEmpty(businesstype))
				ParameterAll += $"&businesstype={businesstype}";

			if (provinceid > 0)
				ParameterAll += $"&provinceid={provinceid}";

			if (amphurid > 0)
				ParameterAll += $"&amphurid={amphurid}";

			if (!String.IsNullOrEmpty(province_name))
				ParameterAll += $"&province_name={province_name}";

			if (!String.IsNullOrEmpty(amphur_name))
				ParameterAll += $"&amphur_name={amphur_name}";

			if (!String.IsNullOrEmpty(branch))
				ParameterAll += $"&branch={branch}";

			if (!String.IsNullOrEmpty(emp_id))
				ParameterAll += $"&emp_id={emp_id}";

			if (!String.IsNullOrEmpty(emp_name))
				ParameterAll += $"&emp_name={emp_name}";

			if (!String.IsNullOrEmpty(assignmentid))
				ParameterAll += $"&assignmentid={assignmentid}";

			if (!String.IsNullOrEmpty(mcenter_code))
				ParameterAll += $"&mcenter_code={mcenter_code}";

			if (!String.IsNullOrEmpty(mcenter_name))
				ParameterAll += $"&mcenter_name={mcenter_name}";

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

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(assigncenter), out var _assigncenter))
				assigncenter = Convert.ToInt16(_assigncenter);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(assignrm), out var _assignrm))
				assignrm = Convert.ToInt16(_assignrm);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(status), out var _status))
				status = Convert.ToInt16(_status);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isshow), out var _isshow))
				isshow = Convert.ToInt16(_isshow);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(CurrentUserId), out var _CurrentUserId))
				CurrentUserId = Convert.ToInt32(_CurrentUserId);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(cif), out var _cif))
				cif = _cif;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(searchtxt), out var _searchtxt))
				searchtxt = _searchtxt;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(juristicnumber), out var _juristicnumber))
				juristicnumber = _juristicnumber;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(chain), out var _chain))
				chain = _chain;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isiccode), out var _isiccode))
				isiccode = _isiccode;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(type), out var _type))
				type = _type;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(businesstype), out var _businesstype))
				businesstype = _businesstype;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(provinceid), out var _province))
				provinceid = Convert.ToInt32(_province); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(amphurid), out var _amphur))
				amphurid = Convert.ToInt32(_amphur); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(branch), out var _branch))
				branch = _branch;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(emp_id), out var _emp_id))
				emp_id = _emp_id;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(emp_name), out var _emp_name))
				emp_name = _emp_name;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(assignmentid), out var _assignmentid))
				assignmentid = _assignmentid;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(mcenter_code), out var _mcenter_code))
				mcenter_code = _mcenter_code;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(mcenter_name), out var _mcenter_name))
				mcenter_name = _mcenter_name;

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
