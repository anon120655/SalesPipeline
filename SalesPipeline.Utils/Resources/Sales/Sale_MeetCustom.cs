using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_MeetCustom : CommonModel
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
        /// บุคคลที่เข้าพบ
        /// </summary>
        //[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
        public string? Name { get; set; }

		/// <summary>
		/// เบอร์ติดต่อ
		/// </summary>
		public string? Tel { get; set; }

		/// <summary>
		/// วันที่เข้าพบ
		/// </summary>
		public DateTime? MeetDate { get; set; }

		/// <summary>
		/// 1=เข้าพบสำเร็จ 2=เข้าพบไม่สำเร็จ
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? MeetId { get; set; }

		/// <summary>
		/// ผลผลิตหลัก
		/// </summary>
		public Guid? Master_YieldId { get; set; }

		/// <summary>
		/// ห่วงโซ่คุณค่า 
		/// </summary>
		public Guid? Master_ChainId { get; set; }

		/// <summary>
		/// 1=นัดเก็บเอกสาร/ประสงค์กู้ 2=เข้าพบอีกครั้ง
		/// </summary>
		public int? NextActionId { get; set; }

		/// <summary>
		/// จำนวนการกู้
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public decimal? LoanAmount { get; set; }

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
	}
}
