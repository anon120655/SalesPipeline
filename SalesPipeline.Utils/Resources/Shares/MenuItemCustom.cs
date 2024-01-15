using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class MenuItemCustom
	{
		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int MenuNumber { get; set; }

		public int? ParentNumber { get; set; }

		public string Name { get; set; } = null!;

		public int Sequence { get; set; }

		public string? Path { get; set; }

		public string? ImageUrl { get; set; }
	}
}
