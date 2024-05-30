using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Loans
{
	public class Loan_AppLoanCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid LoanId { get; set; }

		public Guid Master_Pre_Applicant_LoanId { get; set; }

		public string? Master_Pre_Applicant_LoanName { get; set; }

	}
}
