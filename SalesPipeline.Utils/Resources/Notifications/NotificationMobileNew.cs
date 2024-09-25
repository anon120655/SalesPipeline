using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationMobileNew
	{

		public Message? message { get; set; }

		public class Data
		{
			public string? score { get; set; }
			public string? time { get; set; }
		}

		public class Message
		{
			public string? token { get; set; }
			public Notification? notification { get; set; }
			public Android? android { get; set; }
			public Apns? apns { get; set; }
			public Data? data { get; set; }
		}

		public class Android
		{
			public string? priority { get; set; }
		}

		public class Apns
		{
			public Headers? headers { get; set; }
		}

		public class Headers
		{
			[JsonProperty("apns-priority")]
			public string? apnspriority { get; set; }
		}

		public class Notification
		{
			public string? title { get; set; }
			public string? body { get; set; }
		}
	}
}
