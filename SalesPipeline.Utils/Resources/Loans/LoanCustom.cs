using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Loans
{
	public class LoanCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		/// <summary>
		/// ชื่อสินเชื่อ
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// ประเภทการชำระดอกเบี้ย
		/// </summary>
		public Guid? Master_Pre_Interest_PayTypeId { get; set; }

		public string? Master_Pre_Interest_PayTypeName { get; set; }

		/// <summary>
		/// จำนวนช่วงเวลา
		/// </summary>
		public int? PeriodNumber { get; set; }

		/// <summary>
		/// Risk Premium รายปี
		/// </summary>
		public decimal? RiskPremiumYear { get; set; }

		public virtual ICollection<Loan_PeriodCustom>? Loan_Periods { get; set; }
	}
}
