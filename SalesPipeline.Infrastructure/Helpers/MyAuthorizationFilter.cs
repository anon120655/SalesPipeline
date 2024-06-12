using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class MyAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			// กำหนดเงื่อนไขการรับรองความถูกต้อง
			// ตัวอย่างนี้ให้ทุกคนที่เข้าถึงได้
			return true;
		}
	}
}
