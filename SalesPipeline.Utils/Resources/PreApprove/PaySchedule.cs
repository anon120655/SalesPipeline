using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class PaySchedule
	{
        public PayScheduleFactor? Factor { get; set; }
        public List<PayScheduleItem>? ScheduleItem { get; set; }
    }
}
