using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_StatusCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? CreateByName { get; set; }

		public Guid SaleId { get; set; }

		public int? StatusMainId { get; set; }

		/// <summary>
		/// สถานะการขายหลัก
		/// </summary>
		public string? StatusNameMain { get; set; }

		public int StatusId { get; set; }

		/// <summary>
		/// สถานะการขาย
		/// </summary>
		public string? StatusName { get; set; }

		public string? Description { get; set; }

		public Guid? Master_Reason_CloseSaleId { get; set; }
	}
}
