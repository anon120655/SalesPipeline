using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationAtt.PreApprove;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? Name { get; set; }

		/// <summary>
		/// ประเภทการชำระดอกเบี้ย
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public Guid? Master_Pre_Interest_PayTypeId { get; set; }

		public string? Master_Pre_Interest_PayTypeName { get; set; }

		/// <summary>
		/// จำนวนช่วงเวลา
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? PeriodNumber { get; set; }

		/// <summary>
		/// Risk Premium รายปี
		/// </summary>
		public decimal? RiskPremiumYear { get; set; }

		/// <summary>
		/// เงื่อนไข
		/// </summary>
		public string? Condition { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		[MinLength(1, ErrorMessage = "ต้องมีอย่างน้อย 1 รายการ")]
		public virtual List<Loan_AppLoanCustom>? Loan_AppLoans { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		[MinLength(1, ErrorMessage = "ต้องมีอย่างน้อย 1 รายการ")]
		public virtual List<Loan_BusTypeCustom>? Loan_BusTypes { get; set; }

		public virtual List<Loan_PeriodCustom>? Loan_Periods { get; set; }

	}
}
