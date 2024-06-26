using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.iAuthen
{
	public class iAuthenResponse
	{
		public string? response_status { get; set; }
		public string? response_message { get; set; }
		public ResponseData? response_data { get; set; }

		public class ResponseData
		{
			public int authen_fail_time { get; set; }
			public string? branch_code { get; set; }
			public string? branch_name { get; set; }
			public string? cbs_id { get; set; }
			public string? change_password_url { get; set; }
			public string? create_password_url { get; set; }
			public string? email { get; set; }
			public string? employee_id { get; set; }
			public string? employee_position_id { get; set; }
			public string? employee_position_level { get; set; }
			public string? employee_position_name { get; set; }
			public string? employee_status { get; set; }
			public string? first_name_th { get; set; }
			public bool image_existing { get; set; }
			public string? job_field_id { get; set; }
			public string? job_field_name { get; set; }
			public string? job_id { get; set; }
			public string? job_name { get; set; }
			public string? last_name_th { get; set; }
			public DateTime? lastauthen_timestamp { get; set; }
			public string? mobile_no { get; set; }
			public string? name_en { get; set; }
			public string? org_id { get; set; }
			public string? org_name { get; set; }
			public string? organization_48 { get; set; }
			public string? organization_abbreviation { get; set; }
			public string? organization_upper_id { get; set; }
			public string? organization_upper_id2 { get; set; }
			public string? organization_upper_id3 { get; set; }
			public string? organization_upper_name { get; set; }
			public string? organization_upper_name2 { get; set; }
			public string? organization_upper_name3 { get; set; }
			public bool password_unexpire { get; set; }
			public bool requester_active { get; set; }
			public bool requester_existing { get; set; }
			public DateTime timeresive { get; set; }
			public DateTime timesend { get; set; }
			public string? title_th { get; set; }
			public string? title_th_2 { get; set; }
			public string? user_class { get; set; }
			public bool username_active { get; set; }
			public bool username_existing { get; set; }
			public string? working_status { get; set; }

			//Custom
			public string? Username { get; set; }
		}
	}
}
