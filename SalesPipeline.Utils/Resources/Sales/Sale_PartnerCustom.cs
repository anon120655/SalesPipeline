using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_PartnerCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

		/// <summary>
		/// ชื่อคู่ค้า
		/// </summary>
		public string? FullName { get; set; }

		/// <summary>
		/// ประเภทธุรกิจ
		/// </summary>
		public Guid? Master_BusinessTypeId { get; set; }

		public string? Master_BusinessTypeName { get; set; }

		/// <summary>
		/// ผลผลิตหลัก
		/// </summary>
		public Guid? Master_YieldId { get; set; }

		public string? Master_YieldName { get; set; }

		/// <summary>
		/// ห่วงโซ่คุณค่า 
		/// </summary>
		public Guid? Master_ChainId { get; set; }

		public string? Master_ChainName { get; set; }

		/// <summary>
		/// ขนาดธุรกิจ
		/// </summary>
		public Guid? Master_BusinessSizeId { get; set; }

		public string? Master_BusinessSizeName { get; set; }

		public string? Tel { get; set; }
	}
}
