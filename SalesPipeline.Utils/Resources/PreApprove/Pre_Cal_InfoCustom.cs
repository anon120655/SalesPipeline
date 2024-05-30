using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Cal_InfoCustom : CommonModel
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
		/// คะแนนสูงสุด
		/// </summary>
		public int? HighScore { get; set; }

		/// <summary>
		/// ประเภทผู้ขอสินเชื่อ
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public Guid? Master_Pre_Applicant_LoanId { get; set; }

		public string? Master_Pre_Applicant_LoanName { get; set; }

		/// <summary>
		/// ประเภทธุรกิจ
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public Guid? Master_Pre_BusinessTypeId { get; set; }

		public string? Master_Pre_BusinessTypeName { get; set; }

		public virtual List<Pre_Cal_Info_ScoreCustom>? Pre_Cal_Info_Scores { get; set; }
	}
}
