using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SalesPipeline.Utils.Resources.Notifications.NotificationMobileResponse;

namespace SalesPipeline.Utils.Resources.Phoenixs
{
	public class PhoenixResponse
	{
		public string? status { get; set; }
		public string? message { get; set; }
		public List<Result>? result { get; set; }

		public class Result
		{
			public string? workflow_id { get; set; }
			public string? app_no { get; set; }
			public string? ana_no { get; set; }
			public string? fin_type { get; set; }
			public string? cif_no { get; set; }
			public string? cif_name { get; set; }
			public string? branch_customer { get; set; }
			public string? branch_user { get; set; }
			public string? approve_level { get; set; }
			public string? status_type { get; set; }
			public string? status_code { get; set; }
			public string? create_by { get; set; }
			public string? created_date { get; set; }
			public string? update_by { get; set; }
			public string? update_date { get; set; }
			public string? approve_by { get; set; }
			public string? approve_date { get; set; }
		}
	}
}
