using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Result_ItemCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_ResultId { get; set; }

		/// <summary>
		/// ลำดับ
		/// </summary>
		public int SequenceNo { get; set; }

		/// <summary>
		/// 1=ข้อมูลการขอสินเชื่อ
		/// 2=คุณสมบัติมารตฐาน
		/// 3=คุณสมบัติตามประเภทผู้ขอ
		/// 4=คุณสมบัติตามประเภทธุรกิจ
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// ปัจจัยการวิเคราะห์
		/// </summary>
		public string? AnalysisFactor { get; set; }

		/// <summary>
		/// คุณสมบัติ
		/// </summary>
		public string? Feature { get; set; }

		/// <summary>
		/// คะแนน
		/// </summary>
		public decimal? Score { get; set; }

		/// <summary>
		/// อัตราส่วน
		/// </summary>
		public decimal? Ratio { get; set; }

		/// <summary>
		/// ผลคะแนน
		/// </summary>
		public decimal? ScoreResult { get; set; }

	}
}
