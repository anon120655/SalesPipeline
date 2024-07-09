using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class HistoryLoanModel
	{
        public DateTime? DateApprove { get; set; }
        public string? ProjectLoan { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? LoanPeriod { get; set; }
    }
}
