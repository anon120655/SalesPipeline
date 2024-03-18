using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_MeetCustom
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

		public Guid SaleId { get; set; }

		/// <summary>
		/// บุคคลที่เข้าพบ
		/// </summary>
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
		public Guid? MeetId { get; set; }

		/// <summary>
		/// ผลผลิตหลัก
		/// </summary>
		public Guid? Master_YieldId { get; set; }

		/// <summary>
		/// ห่วงโซ่คุณค่า 
		/// </summary>
		public Guid? Master_ChainId { get; set; }

		/// <summary>
		/// 1=นัดเก็บเอกสาร/ประสงค์กู้
		/// </summary>
		public int? NextActionId { get; set; }

		/// <summary>
		/// จำนวนการกู้
		/// </summary>
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
