using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationMobile
	{
		public string? to { get; set; }
        public string? priority { get; set; }
        public Notification? notification { get; set; }
        public DataMore? data { get; set; }

        public class Notification
		{
			public string? title { get; set; }
			public string? subtitle { get; set; }
			public int vibrate { get; set; }
			public string? body { get; set; }
			public string? icon { get; set; }
			public string? sound { get; set; }
			public string? message { get; set; }
			public string? badge { get; set; }

			[JsonProperty("content-available")]
			public int contentavailable { get; set; }

			[JsonProperty("force-start")]
			public int forcestart { get; set; }

			[JsonProperty("no-cache")]
			public int nocache { get; set; }
		}

        public class DataMore
        {
            public Guid? SaleId { get; set; }
            public int? EventId { get; set; }
            public string? EventName { get; set; }
            public int? FromUserId { get; set; }
            public string? FromUserName { get; set; }
            public int? ToUserId { get; set; }
            public string? ToUserName { get; set; }
            public string? ActionName1 { get; set; }
            public string? ActionName2 { get; set; }
        }
    }
}
