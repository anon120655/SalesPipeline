using SalesPipeline.Utils.Resources.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public class AppSettings
	{
		public string? Secret { get; set; }
		public string? SiteUpdate { get; set; } = $"2567-02-01";
		public string? ServerSite { get; set; }
		public string? baseUriApi { get; set; }
		public string? baseUriWeb { get; set; }
		public string? ContentRootPath { get; set; }
        public RequestResponseLoggerOption? RequestResponseLogger { get; set; }
        public LineNotifys? LineNotify { get; set; }

		public class LineNotifys
		{
			public string? baseUri { get; set; }
			public string? Token { get; set; }
		}

		public string? Version
		{
			get
			{
				if (!String.IsNullOrEmpty(SiteUpdate))
				{
					return SiteUpdate.Replace("-", "");
				}

				return SiteUpdate;
			}
		}

	}

}
