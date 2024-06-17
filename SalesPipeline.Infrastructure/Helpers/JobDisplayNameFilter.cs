using Hangfire.Client;
using Hangfire.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class JobDisplayNameFilter : JobFilterAttribute, IClientFilter
	{
		private readonly string _displayName;

		public JobDisplayNameFilter(string displayName)
		{
			_displayName = displayName;
		}

		public void OnCreating(CreatingContext filterContext)
		{
			filterContext.SetJobParameter("DisplayName", _displayName);
		}

		public void OnCreated(CreatedContext filterContext)
		{
		}
	}
}
