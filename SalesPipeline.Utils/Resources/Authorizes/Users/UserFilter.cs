using Microsoft.AspNetCore.WebUtilities;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class UserFilter : PagerFilter
	{
		public int id { get; set; }
		public int? createby { get; set; }
		public short? status { get; set; }
		public string? searchtxt { get; set; }
		public string? employeeid { get; set; }
		public string? fullname { get; set; }
		public string? type { get; set; }
		public int? branchid { get; set; }
		public int? roleid { get; set; }
		public string? spositions { get; set; }
		public List<string?>? PositionsList { get; set; }
		public string? suserlevels { get; set; }
		public List<string?>? SUserLevelsList { get; set; }

		public string SetParameter(bool? isPage = null)
		{
			string? ParameterAll = String.Empty;

			if (isPage.HasValue && isPage.Value)
				ParameterAll = $"page={page}";

			ParameterAll += $"&pagesize={pagesize}";

			if (id > 0)
				ParameterAll += $"&id={id}";

			if (createby > 0)
				ParameterAll += $"&createby={createby}";

			if (status.HasValue)
				ParameterAll += $"&status={status}";

			if (!String.IsNullOrEmpty(searchtxt))
				ParameterAll += $"&searchtxt={searchtxt}";

			if (!String.IsNullOrEmpty(employeeid))
				ParameterAll += $"&employeeid={employeeid}";

			if (!String.IsNullOrEmpty(fullname))
				ParameterAll += $"&fullname={fullname}";

			if (!String.IsNullOrEmpty(type))
				ParameterAll += $"&type={type}";

			if (branchid > 0)
				ParameterAll += $"&branchid={branchid}";

			if (roleid > 0)
				ParameterAll += $"&roleid={roleid}";

			if (PositionsList != null && PositionsList.Count(x => !String.IsNullOrEmpty(x)) > 0)
			{
				string joined = string.Join(",", PositionsList);
				ParameterAll += $"&spositions={joined}";
			}
			if (SUserLevelsList != null && SUserLevelsList.Count(x => !String.IsNullOrEmpty(x)) > 0)
			{
				string joined = string.Join(",", SUserLevelsList);
				ParameterAll += $"&suserlevels={joined}";
			}
			return ParameterAll;
		}

		public void SetUriQuery(string uriQuery)
		{
			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(page), out var _page))
				page = Convert.ToInt32(_page);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(pagesize), out var _pagesize))
				pagesize = Convert.ToInt32(_pagesize);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(status), out var _IsActive))
				status = Convert.ToInt16(_IsActive);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(CurrentUserId), out var _CurrentUserId))
				CurrentUserId = Convert.ToInt32(_CurrentUserId);

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(searchtxt), out var _searchtxt))
				searchtxt = _searchtxt;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(employeeid), out var _employeeid))
				employeeid = _employeeid;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(fullname), out var _fullname))
				fullname = _fullname;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(type), out var _type))
				type = _type;

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(id), out var _id))
			{
				if (int.TryParse(_id, out int __id))
					id = __id;
			}

			if (QueryHelpers.ParseQuery(uriQuery).TryGetValue(nameof(createby), out var _createby))
			{
				if (int.TryParse(_createby, out int __createby))
					createby = __createby;
			}

		}

	}
}
