using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Loans
{
	public class Loan_PeriodCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		/// <summary>
		/// FK สินเชื่อ
		/// </summary>
		public Guid LoanId { get; set; }

		/// <summary>
		/// ระยะที่
		/// </summary>
		public int PeriodNo { get; set; }

		/// <summary>
		/// FK ประเภทอัตราดอกเบี้ย
		/// </summary>
		public Guid? Master_Pre_Interest_RateTypeId { get; set; }

		public string? Master_Pre_Interest_RateTypeName { get; set; }

		public string? Master_Pre_Interest_RateTypeCode { get; set; }

		/// <summary>
		/// 1=เพิ่ม 2=ลบ
		/// </summary>
		public int? SpecialType { get; set; }

		/// <summary>
		/// ค่าเพิ่มลบดอกเบี้ย %
		/// </summary>
		public decimal? SpecialRate { get; set; }

		/// <summary>
		/// อัตราดอกเบี้ย %
		/// </summary>
		public decimal? RateValue { get; set; }

		/// <summary>
		/// เริ่มปีที่
		/// </summary>
		public int? StartYear { get; set; }

		public virtual LoanCustom? Loan { get; set; }

		//Custom
		public decimal? RateValueOriginal { get; set; }

	}
}
