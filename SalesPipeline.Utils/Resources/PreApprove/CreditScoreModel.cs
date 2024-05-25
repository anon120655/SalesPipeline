using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class CreditScoreModel
	{
        public string? Level { get; set; }
        public string? CreditScore { get; set; }
        public string? CreditScoreColor { get; set; }
		public string? Grade { get; set; } = null!;
		public string? LimitMultiplier { get; set; }
		public string? RateMultiplier { get; set; }
	}
}
