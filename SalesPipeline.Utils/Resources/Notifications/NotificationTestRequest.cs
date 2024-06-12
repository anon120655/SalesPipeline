using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationTestRequest
	{
		public string Message { get; set; } = null!;
		public DateTimeOffset NotifyAt { get; set; }
	}
}
