
using SalesPipeline.Utils.Resources.Shares;
using System.ComponentModel.DataAnnotations;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_CalCustom : CommonModel
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

		public string? Name { get; set; }

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

		/// <summary>
		/// 1=แสดงเฉพาะคะแนนรวม
		/// </summary>
		public int? DisplayResultType { get; set; }

		public virtual List<Pre_Cal_Fetu_AppCustom>? Pre_Cal_Fetu_Apps { get; set; }

		public virtual List<Pre_Cal_Fetu_BuCustom>? Pre_Cal_Fetu_Bus { get; set; }

		public virtual List<Pre_Cal_Fetu_StanCustom>? Pre_Cal_Fetu_Stans { get; set; }

		public virtual List<Pre_Cal_InfoCustom>? Pre_Cal_Infos { get; set; }

		public virtual List<Pre_Cal_WeightFactorCustom>? Pre_Cal_WeightFactors { get; set; }

	}
}
