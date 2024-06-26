using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_Contact_InfoCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

		/// <summary>
		/// ชื่อผู้ติดต่อ
		/// </summary>
		public string? FullName { get; set; }

		/// <summary>
		/// ตำแหน่ง
		/// </summary>
		public string? Position { get; set; }

		public string? Tel { get; set; }

		public string? Email { get; set; }

        /// <summary>
        /// 1=ฟอร์มเพิ่มลูกค้า backend
        /// </summary>
        public short? Createdfrom { get; set; }
    }
}
