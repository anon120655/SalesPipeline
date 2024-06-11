using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationMobileResponse
	{
		public long multicast_id { get; set; }
		public int success { get; set; }
		public int failure { get; set; }
		public int canonical_ids { get; set; }
		public List<Result>? results { get; set; }

		public class Result
		{
			public string? message_id { get; set; }
		}
	}
}
