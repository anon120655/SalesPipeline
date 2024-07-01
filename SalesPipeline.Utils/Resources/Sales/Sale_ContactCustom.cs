using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_ContactCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

        public Guid? SaleReplyId { get; set; }

        /// <summary>
        /// ชื่อผู้ติดต่อ
        /// </summary>
        //[Required(ErrorMessage = "ระบุชื่อผู้ติดต่อ")]
        public string? Name { get; set; }

		/// <summary>
		/// เบอร์ติดต่อ
		/// </summary>
		//[Required(ErrorMessage = "ระบุเบอร์ติดต่อ")]
		public string? Tel { get; set; }

		/// <summary>
		/// วันที่ติดต่อ
		/// </summary>
		//[Required(ErrorMessage = "ระบุวันที่ติดต่อ")]
		public DateTime? ContactDate { get; set; }

		/// <summary>
		/// 1=รับสาย 2=ไม่รับสาย
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุชื่อผู้ติดต่อ")]
		public int? ContactResult { get; set; }

		/// <summary>
		/// 1=ทำการนัดหมาย 2=ติดต่ออีกครั้ง 3=ส่งกลับรายการ
		/// </summary>
		//[Required(ErrorMessage = "Next Action")]
		public int? NextActionId { get; set; }

		/// <summary>
		/// วันที่นัดหมาย
		/// </summary>
		public DateTime? AppointmentDate { get; set; }

		/// <summary>
		/// เวลาที่นัดหมาย
		/// </summary>
		public TimeOnly? AppointmentTime { get; set; }

		/// <summary>
		/// สถานที่
		/// </summary>
		public string? Location { get; set; }

		/// <summary>
		/// บันทึกเพิ่มเติม
		/// </summary>
		public string? Note { get; set; }

		/// <summary>
		/// 2=ไม่ประสงค์กู้
		/// </summary>
		public int? DesireLoanId { get; set; }

		public Guid? Master_Reason_CloseSaleId { get; set; }
	}
}
