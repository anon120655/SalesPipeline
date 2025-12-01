using Hangfire.Client;
using Hangfire.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class JobDisplayNameFilter : JobFilterAttribute, IClientFilter
    {
        private readonly string _displayName;
        public JobDisplayNameFilter(string displayName)
        {
            _displayName = displayName;
        }
        public void OnCreating(CreatingContext context)
        {
            context.SetJobParameter("DisplayName", _displayName);
        }
        public void OnCreated(CreatedContext context)
        {
        }
    }
}