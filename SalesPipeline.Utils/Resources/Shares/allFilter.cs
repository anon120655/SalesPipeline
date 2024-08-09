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
		public Guid? saleid { get; set; }
		public int? userid { get; set; }
		public int? assigncenter { get; set; }
		public int? assignrm { get; set; }
        public string? assignrm_name { get; set; }
		public string? contact_name { get; set; }
        public DateTime? contactstartdate { get; set; }
        public string? code { get; set; }
		public string? rolecode { get; set; }
		public string? psalecode { get; set; }
		public short? status { get; set; }
		public short? isshow { get; set; }
		public string? cif { get; set; }
		public string? searchtxt { get; set; }
		public string? juristicnumber { get; set; }
		public string? type { get; set; }
		public string? chain { get; set; }
		public string? isiccode { get; set; }
		public string? businesstype { get; set; }
        public string? loantypeid { get; set; }
        public int? provinceid { get; set; }
		public int? amphurid { get; set; }
		public string? province_name { get; set; }
		public string? amphur_name { get; set; }
		public string? branchid { get; set; }
		public string? branch_name { get; set; }
		public string? emp_id { get; set; }
		public string? emp_name { get; set; }
		public string? assignmentid { get; set; }
		public string? mcenter_code { get; set; }
		public string? mcenter_name { get; set; }
		public short? isloanamount { get; set; }
		public decimal? loanamount { get; set; }
		public decimal? amounttarget { get; set; }
		public short? achieve_goal { get; set; }
		public short? isclosesale { get; set; }
		public short? isconsidered { get; set; }
		public short? isoverdue { get; set; }
		public string? reason { get; set; }
		public string? contact { get; set; }
		public string? meet { get; set; }
		public string? document { get; set; }
		public string? loantobranchreg { get; set; }
		public string? branchregtocenbranch { get; set; }
		public string? cenbranchtorm { get; set; }
		public string? closesale { get; set; }
		public string? val1 { get; set; }
		public string? val2 { get; set; }
		public string? val3 { get; set; }
		public string? ids { get; set; }
		public string? ids2 { get; set; }
		public string? ids3 { get; set; }
		public string? sort { get; set; }
		public string? year { get; set; }
		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
		public DateTime? returndate { get; set; }
		public DateTime? createdate { get; set; }
		//public List<string?>? Selecteds { get; set; }
		//public List<string?>? Selecteds2 { get; set; }
		//public List<string?>? Selecteds3 { get; set; } AssUserId
		public List<string?>? DepBranchs { get; set; }
		public List<string?>? Provinces { get; set; }
		public List<string?>? Branchs { get; set; }
		public List<string?>? RMUsers { get; set; }
		public List<string?>? AssUsers { get; set; }
		public List<string?>? Years { get; set; }
        public List<string?>? StatusSales { get; set; }
		public short? isScheduledJob { get; set; }
		public short? isAll { get; set; }
		public short? isMatchCal { get; set; }
		public string? preapploanid { get; set; }
		public short? isassigncenter { get; set; }
		public short? isassignrm { get; set; }

		public string SetParameter(bool? isPage = null)
		{
			string? ParameterAll = String.Empty;

			if (isPage.HasValue && isPage.Value)
				ParameterAll = $"page={page}";

			ParameterAll += $"&pagesize={pagesize}";

			if (id != Guid.Empty)
				ParameterAll += $"&id={id}";

			if (saleid != Guid.Empty)
				ParameterAll += $"&saleid={saleid}";

			if (customerid.HasValue && customerid != Guid.Empty)
				ParameterAll += $"&customerid={customerid}";

			if (statussaleid > 0)
				ParameterAll += $"&statussaleid={statussaleid}";

			if (userid > 0)
				ParameterAll += $"&userid={userid}";

			if (assigncenter > 0)
				ParameterAll += $"&assigncenter={assigncenter}";

			if (assignrm > 0)
				ParameterAll += $"&assignrm={assignrm}";

			if (!String.IsNullOrEmpty(assignrm_name))
				ParameterAll += $"&assignrm_name={assignrm_name}";

			if (!String.IsNullOrEmpty(contact_name))
				ParameterAll += $"&contact_name={contact_name}";

			if (contactstartdate.HasValue)
				ParameterAll += $"&contactstartdate={GeneralUtils.DateToStrParameter(contactstartdate)}";

			if (!String.IsNullOrEmpty(code))
				ParameterAll += $"&code={code}";

			if (!String.IsNullOrEmpty(rolecode))
				ParameterAll += $"&rolecode={rolecode}";

			if (!String.IsNullOrEmpty(psalecode))
				ParameterAll += $"&psalecode={psalecode}";

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

			if (!String.IsNullOrEmpty(loantypeid))
				ParameterAll += $"&loantypeid={loantypeid}";

			if (provinceid > 0)
				ParameterAll += $"&provinceid={provinceid}";

			if (amphurid > 0)
				ParameterAll += $"&amphurid={amphurid}";

			if (!String.IsNullOrEmpty(province_name))
				ParameterAll += $"&province_name={province_name}";

			if (!String.IsNullOrEmpty(amphur_name))
				ParameterAll += $"&amphur_name={amphur_name}";

			if (!String.IsNullOrEmpty(branchid))
				ParameterAll += $"&branchid={branchid}";

			if (!String.IsNullOrEmpty(branch_name))
				ParameterAll += $"&branch_name={branch_name}";

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

			if (loanamount > 0)
				ParameterAll += $"&loanamount={loanamount}";

			if (amounttarget > 0)
				ParameterAll += $"&amounttarget={amounttarget}";

			if (achieve_goal.HasValue)
				ParameterAll += $"&achieve_goal={achieve_goal}";

			if (isloanamount.HasValue)
				ParameterAll += $"&isloanamount={isloanamount}";

			if (isclosesale.HasValue)
				ParameterAll += $"&isclosesale={isclosesale}";

			if (isconsidered.HasValue)
				ParameterAll += $"&isconsidered={isconsidered}";

			if (isoverdue.HasValue)
				ParameterAll += $"&isoverdue={isoverdue}";

			if (!String.IsNullOrEmpty(reason))
				ParameterAll += $"&reason={reason}";

			if (!String.IsNullOrEmpty(contact))
				ParameterAll += $"&contact={contact}";

			if (!String.IsNullOrEmpty(meet))
				ParameterAll += $"&meet={meet}";

			if (!String.IsNullOrEmpty(document))
				ParameterAll += $"&document={document}";

			if (!String.IsNullOrEmpty(loantobranchreg))
				ParameterAll += $"&loantobranchreg={loantobranchreg}";

			if (!String.IsNullOrEmpty(branchregtocenbranch))
				ParameterAll += $"&branchregtocenbranch={branchregtocenbranch}";

			if (!String.IsNullOrEmpty(cenbranchtorm))
				ParameterAll += $"&cenbranchtorm={cenbranchtorm}";

			if (!String.IsNullOrEmpty(closesale))
				ParameterAll += $"&closesale={closesale}";

			if (!String.IsNullOrEmpty(val1))
				ParameterAll += $"&val1={val1}";

			if (!String.IsNullOrEmpty(val2))
				ParameterAll += $"&val2={val2}";

			if (!String.IsNullOrEmpty(val3))
				ParameterAll += $"&val3={val3}";

			if (!String.IsNullOrEmpty(sort))
				ParameterAll += $"&sort={sort}";

			if (!String.IsNullOrEmpty(year))
				ParameterAll += $"&year={year}";

			if (DepBranchs?.Count > 0)
			{
				string joined = string.Join(",", DepBranchs);
				ParameterAll += $"&depbranchs={joined}";
			}

			if (Provinces?.Count > 0)
			{
				string joined = string.Join(",", Provinces);
				ParameterAll += $"&provinces={joined}";
			}

			if (Branchs?.Count > 0)
			{
				string joined = string.Join(",", Branchs);
				ParameterAll += $"&branchs={joined}";
			}

			if (RMUsers?.Count > 0)
			{
				string joined = string.Join(",", RMUsers);
				ParameterAll += $"&rmusers={joined}";
			}

			if (AssUsers?.Count > 0)
			{
				string joined = string.Join(",", AssUsers);
				ParameterAll += $"&assuser={joined}";
			}

			if (Years?.Count > 0)
			{
				string joined = string.Join(",", Years);
				ParameterAll += $"&years={joined}";
			}

			if (StatusSales?.Count > 0)
			{
				string joined = string.Join(",", StatusSales);
				ParameterAll += $"&statussales={joined}";
			}

			if (startdate.HasValue)
				ParameterAll += $"&startdate={GeneralUtils.DateToStrParameter(startdate)}";

			if (enddate.HasValue)
				ParameterAll += $"&enddate={GeneralUtils.DateToStrParameter(enddate)}";

			if (returndate.HasValue)
				ParameterAll += $"&returndate={GeneralUtils.DateToStrParameter(returndate)}";

			if (createdate.HasValue)
				ParameterAll += $"&createdate={GeneralUtils.DateToStrParameter(createdate)}";

			if (isScheduledJob.HasValue)
				ParameterAll += $"&isScheduledJob={isScheduledJob}";

			if (isAll.HasValue)
				ParameterAll += $"&isAll={isAll}";

			if (isMatchCal.HasValue)
				ParameterAll += $"&isMatchCal={isMatchCal}";

			if (!String.IsNullOrEmpty(preapploanid))
				ParameterAll += $"&preapploanid={preapploanid}";

			if (isassigncenter.HasValue)
				ParameterAll += $"&isassigncenter={isassigncenter}";

			if (isassignrm.HasValue)
				ParameterAll += $"&isassignrm={isassignrm}";

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

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(assignrm_name), out var _assignrm_name))
				assignrm_name = _assignrm_name;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(contact_name), out var _contact_name))
				contact_name = _contact_name;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("contactstartdate", out var _contactstartdate))
				contactstartdate = GeneralUtils.DateNotNullToEn(_contactstartdate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(code), out var _code))
				code = _code;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(rolecode), out var _rolecode))
				rolecode = _rolecode;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(psalecode), out var _psalecode))
				psalecode = _psalecode;

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

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(loantypeid), out var _loantypeid))
				loantypeid = _loantypeid;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(provinceid), out var _province))
				provinceid = Convert.ToInt32(_province); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(amphurid), out var _amphur))
				amphurid = Convert.ToInt32(_amphur); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(branchid), out var _branchid))
				branchid = _branchid;

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

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(amounttarget), out var _amounttarget))
				amounttarget = Convert.ToDecimal(_amounttarget); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(achieve_goal), out var _achieve_goal))
				achieve_goal = Convert.ToInt16(_achieve_goal);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(loanamount), out var _loanamount))
				loanamount = Convert.ToDecimal(_loanamount); ;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isloanamount), out var _isloanamount))
				isloanamount = Convert.ToInt16(_isloanamount);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isclosesale), out var _isclosesale))
				isclosesale = Convert.ToInt16(_isclosesale);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isconsidered), out var _isconsidered))
				isconsidered = Convert.ToInt16(_isconsidered);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(isoverdue), out var _isoverdue))
				isoverdue = Convert.ToInt16(_isoverdue);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(reason), out var _reason))
				reason = _reason;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(contact), out var _contact))
				contact = _contact;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(meet), out var _meet))
				meet = _meet;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(document), out var _document))
				document = _document;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(loantobranchreg), out var _loantobranchreg))
				loantobranchreg = _loantobranchreg;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(branchregtocenbranch), out var _branchregtocenbranch))
				branchregtocenbranch = _branchregtocenbranch;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(cenbranchtorm), out var _cenbranchtorm))
				cenbranchtorm = _cenbranchtorm;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(closesale), out var _closesale))
				closesale = _closesale;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val1), out var _val1))
				val1 = _val1;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val2), out var _val2))
				val2 = _val2;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(val3), out var _val3))
				val3 = _val3;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(sort), out var _sort))
				sort = _sort;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(year), out var _year))
				year = _year;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("startdate", out var _startdate))
				startdate = GeneralUtils.DateNotNullToEn(_startdate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("enddate", out var _enddate))
				enddate = GeneralUtils.DateNotNullToEn(_enddate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("returndate", out var _returndate))
				returndate = GeneralUtils.DateNotNullToEn(_returndate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("createdate", out var _createdate))
				createdate = GeneralUtils.DateNotNullToEn(_createdate, "yyyy-MM-dd", Culture: "en-US");

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("depbranchs", out var _DepBranchs))
			{
				DepBranchs = new();
				List<string> lists = _DepBranchs.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						DepBranchs.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("provinces", out var _Provinces))
			{
				Provinces = new();
				List<string> lists = _Provinces.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						Provinces.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("branchs", out var _Branchs))
			{
				Branchs = new();
				List<string> lists = _Branchs.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						Branchs.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("rmusers", out var _RMUsers))
			{
				RMUsers = new();
				List<string> lists = _RMUsers.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						RMUsers.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("assusers", out var _AssUsers))
			{
				AssUsers = new();
				List<string> lists = _AssUsers.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						AssUsers.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("years", out var _Years))
			{
				Years = new();
				List<string> lists = _Years.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						Years.Add(item);
					}
				}
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue("statussales", out var _StatusSales))
			{
				StatusSales = new();
				List<string> lists = _StatusSales.ToString().Split(',').ToList<string>();
				if (lists.Count > 0)
				{
					foreach (var item in lists)
					{
						StatusSales.Add(item);
					}
				}
			}

		}

	}
}
