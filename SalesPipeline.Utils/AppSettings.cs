using SalesPipeline.Utils.Resources.Loggers;

namespace SalesPipeline.Utils
{
	public class AppSettings
	{
		public string? Secret { get; set; }
		public string? SiteUpdate { get; set; } = $"2567-08-04";
		public string ServerSite { get; set; } = null!;
		public string? baseUriApi { get; set; }
		public string? baseUriWeb { get; set; }
		public string? ContentRootPath { get; set; }
		public RequestResponseLoggerOption? RequestResponseLogger { get; set; }
        public LineNotifys? LineNotify { get; set; }
		public EmailSetting? EmailConfig { get; set; }
		public iAuthens? iAuthen { get; set; }
		public NotiMobiles? NotiMobile { get; set; }
		public Databases? Database { get; set; }

		public class LineNotifys
		{
			public string? baseUri { get; set; }
			public string? Token { get; set; }
		}

		public class EmailSetting
		{
			public string? SenderName { get; set; }
			public string Sender { get; set; } = null!;
			public string Password { get; set; } = null!;
			public string? MailServer { get; set; }
			public int MailPort { get; set; }
			public bool IsSentMail { get; set; }
			public string? DefualtMail { get; set; }
			public bool IsSentMailCc { get; set; }
			public string? DefualtMailCc { get; set; }
		}

		public class iAuthens
		{
			public bool IsConnect { get; set; }
			public string? baseUri { get; set; }
			public string? ApiKey { get; set; }
			public string? IPAddress { get; set; }
			public string? Reference_ID { get; set; }
		}

		public class NotiMobiles
		{
			public string? baseUri { get; set; }
			public string? ApiKey { get; set; }
            public int NotiBeforeMinutes { get; set; }
        }

		public class Databases
		{
			public string BackupDatabaseDir { get; set; } = null!;
			public string MySqlDumpPath { get; set; } = null!;
			public string UserDB { get; set; } = null!;
			public string PasswordDB { get; set; } = null!;
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
