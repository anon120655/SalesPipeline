using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class ResourceEmail
	{
		public ResourceEmail()
		{
			Builder = new BodyBuilder();
		}

		public int? CurrentUserId { get; set; }
		public string? SenderName { get; set; }
		public string Sender { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? Subject { get; set; }
		public string? Message { get; set; }
		public string? MailServer { get; set; }
		public int MailPort { get; set; }
		public BodyBuilder Builder { get; set; }
		public List<string>? CcList { get; set; }
		public Guid? TemplateId { get; set; }
		public bool IsCompleted { get; set; }
		public string? StatusMessage { get; set; }
		public string? PkId { get; set; }
	}
}
