using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class SaleCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? CreateByName { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? UpdateByName { get; set; }

		public Guid CustomerId { get; set; }

		/// <summary>
		/// ชื่อบริษัท
		/// </summary>
		public string? CompanyName { get; set; }

		/// <summary>
		/// ผู้รับผิดชอบ
		/// </summary>
		public string? ResponsibleName { get; set; }

		public int StatusSaleId { get; set; }

		/// <summary>
		/// สถานะการขาย
		/// </summary>
		public string? StatusSaleName { get; set; }

		/// <summary>
		/// รายละเอียด
		/// </summary>
		public string? StatusDescription { get; set; }

		/// <summary>
		/// วันที่นัดหมาย
		/// </summary>
		public DateTime? DateAppointment { get; set; }

		/// <summary>
		/// เปอร์เซ็นโอกาสกู้ผ่าน
		/// </summary>
		public int? PercentChanceLoanPass { get; set; }

		public virtual CustomerCustom? Customer { get; set; }

		public virtual List<Sale_StatusCustom>? Sale_Statuses { get; set; }

	}
}
