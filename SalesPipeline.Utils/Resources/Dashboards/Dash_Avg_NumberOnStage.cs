using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class Dash_Avg_NumberOnStage
	{
		public double WaitContact { get; set; }
		public double Contact { get; set; }
		public double WaitMeet { get; set; }
		public double Meet { get; set; }
        public double Document { get; set; }
        public double Result { get; set; }
        public double CloseSaleFail { get; set; }
	}
}
